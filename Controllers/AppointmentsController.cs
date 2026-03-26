using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using vita_care.Features.Appointments.Commands;
using vita_care.Features.Appointments.Queries;
using vita_care.Models;
using vita_care.Repositories;
using vita_care.Services;

namespace vita_care.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(
            IMediator mediator,
            IAuthService authService,
            IAppointmentRepository appointmentRepository)
        {
            _mediator = mediator;
            _authService = authService;
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<Appointment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (!await _authService.HasRoleAsync(User, ["receptionist", "admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only receptionist and admin can access all appointments." });
            }

            var query = new GetAppointmentsQuery
            {
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("stats")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AppointmentStats), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppointmentStats([FromQuery] string email)
        {
            var result = await _mediator.Send(new GetAppointmentStatsQuery { Email = email });
            return Ok(result);
        }

        [HttpGet("stats/all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AppointmentStats), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAppointmentStats()
        {
            if (!await _authService.HasRoleAsync(User, ["receptionist", "admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only staff can access global statistics." });
            }

            var result = await _mediator.Send(new GetAllAppointmentStatsQuery());
            return Ok(result);
        }

        [HttpGet("by-email")]
        [ProducesResponseType(typeof(PaginatedResult<Appointment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAppointmentsByEmail(
            [FromQuery] string email,
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var jwtEmail = _authService.GetUserEmail(User);

            if (jwtEmail != email || !await _authService.HasRoleAsync(User, ["patient","receptionist", "admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "You can only view your own appointments." });
            }

            var query = new GetAppointmentsByEmailQuery
            {
                Email = email,
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var appointmentId = await _mediator.Send(command);
            return Ok(new { Id = appointmentId });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand command)
        {
            // First check if appointment exists and obtain the owner email
            var existing = await _appointmentRepository.GetAppointmentByIdAsync(command.Id, default);
            if (existing == null) return NotFound();

            if (!await _authService.HasRoleAsync(User, ["patient","receptionist", "admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "You can only update your own appointments." });
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var existing = await _appointmentRepository.GetAppointmentByIdAsync(id, default);
            if (existing == null) return NotFound();

            var result = await _mediator.Send(new DeleteAppointmentCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] int status)
        {
            var existing = await _appointmentRepository.GetAppointmentByIdAsync(id, default);
            if (existing == null) return NotFound();


            // Status transition flow validation
            var currentStatus = (AppointmentStatus)existing.Status.Value;
            var targetStatus = (AppointmentStatus)status;

            bool isValidTransition = false;

            if (currentStatus == AppointmentStatus.Requested)
            {
                if (targetStatus == AppointmentStatus.Approved || targetStatus == AppointmentStatus.Canceled)
                    isValidTransition = true;
            }
            else if (currentStatus == AppointmentStatus.Approved)
            {
                if (targetStatus == AppointmentStatus.Visited || targetStatus == AppointmentStatus.NotVisited)
                    isValidTransition = true;
            }

            if (!isValidTransition)
            {
                return BadRequest(new { Message = $"Invalid status transition from {existing.Status.ViewValue} to {targetStatus}." });
            }

            var result = await _mediator.Send(new ChangeAppointmentStatusCommand { Id = id, Status = status });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
