using MediatR;
using Microsoft.AspNetCore.Mvc;
using vita_care.Features.Appointments.Commands;
using vita_care.Features.Appointments.Queries;
using vita_care.Models;

namespace vita_care.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<Appointment>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAppointmentsQuery
            {
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-email")]
        [ProducesResponseType(typeof(PaginatedResult<Appointment>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppointmentsByEmail(
            [FromQuery] string email,
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
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
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var result = await _mediator.Send(new DeleteAppointmentCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] int status)
        {
            var result = await _mediator.Send(new ChangeAppointmentStatusCommand { Id = id, Status = status });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
