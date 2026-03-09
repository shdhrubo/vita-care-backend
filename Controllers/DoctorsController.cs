using MediatR;
using Microsoft.AspNetCore.Mvc;
using vita_care.Features.Doctors.Commands;

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

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDoctor), new { id = doctorId }, doctorId);
        }
    }
}
