using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using vita_care.Features.Doctors.Commands;
using vita_care.Features.Doctors.Queries;
using vita_care.Models;
using vita_care.Services;

namespace vita_care.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public DoctorsController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<Doctor>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetDoctorsQuery
            {
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Doctor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            if (!await _authService.HasRoleAsync(User, ["patient","receptionist","admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "only logged in user can get access." });
            }
            var doctor = await _mediator.Send(new GetDoctorByIdQuery { Id = id });
            
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            if (!await _authService.HasRoleAsync(User, ["admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only admins can add new doctors." });
            }

            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDoctor), new { id = doctorId }, doctorId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorCommand command)
        {
            if (!await _authService.HasRoleAsync(User, ["admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only admins can update doctor information." });
            }

            await _mediator.Send(command);
            return NoContent();
        }

    }
}
