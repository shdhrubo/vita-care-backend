using MediatR;
using Microsoft.AspNetCore.Mvc;
using vita_care.Features.Users.Commands;
using vita_care.Features.Users.Queries;
using vita_care.Models;

namespace vita_care.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<UserInformation>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersQuery
            {
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        
        [HttpPost("upsert")]
        [ProducesResponseType(typeof(vita_care.Features.Users.Commands.UpsertUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpsertUser([FromBody] UpsertUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
