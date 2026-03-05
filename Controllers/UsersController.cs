using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Retrieves paginated user information with optional filtering.
        /// </summary>
        /// <param name="search">Filter by Name or Email (case-insensitive)</param>
        /// <param name="role">Filter by specific Role (e.g., "admin", "patient")</param>
        /// <param name="pageNumber">1-indexed page number</param>
        /// <param name="pageSize">Number of items per page</param>
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

        /// <summary>
        /// Creates a new user or updates an existing one by email.
        /// </summary>
        [HttpPost("upsert")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpsertUser([FromBody] vita_care.Features.Users.Commands.UpsertUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
