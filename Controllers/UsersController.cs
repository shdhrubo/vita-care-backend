using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using vita_care.Features.Users.Commands;
using vita_care.Features.Users.Queries;
using vita_care.Models;

namespace vita_care.Controllers
{
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(typeof(UpsertUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpsertUser([FromBody] UpsertUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("roles")]
        [ProducesResponseType(typeof(UpsertUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null) return NotFound(new { Message = $"User with email '{command.Email}' not found." });
            return Ok(result);
        }
    }
}
