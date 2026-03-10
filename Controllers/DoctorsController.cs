using MediatR;
using Microsoft.AspNetCore.Mvc;
using vita_care.Features.Doctors.Commands;
using vita_care.Features.Doctors.Queries;
using vita_care.Models;

namespace vita_care.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
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
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var doctor = await _mediator.Send(new GetDoctorByIdQuery { Id = id });
            
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDoctor), new { id = doctorId }, doctorId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
