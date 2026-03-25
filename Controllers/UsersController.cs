using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using vita_care.Features.Users.Commands;
using vita_care.Features.Users.Queries;
using vita_care.Models;
using vita_care.Services;

namespace vita_care.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public UsersController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

       
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<UserInformation>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (!await _authService.HasRoleAsync(User, ["admin", "receptionist"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only staff can access the user list." });
            }

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpsertUser([FromBody] UpsertUserCommand command)
        {
            var jwtEmail = _authService.GetUserEmail(User);

            if (jwtEmail != command.Email)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "You can only sync your own profile." });
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("roles")]
        [ProducesResponseType(typeof(UpsertUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            // Must be admin or receptionist to even access this
            if (!await _authService.HasRoleAsync(User, ["admin", "receptionist"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Insufficient permissions to modify roles." });
            }

            // Validation Rule: ONLY Admin can add or remove the 'admin' role
            var modifyingAdminRole = command.AddedRoles.Contains("admin", StringComparer.OrdinalIgnoreCase) || 
                                     command.RemovedRoles.Contains("admin", StringComparer.OrdinalIgnoreCase);

            if (modifyingAdminRole && !await _authService.HasRoleAsync(User, ["admin"]))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Only admins can add or remove the admin role." });
            }

            var result = await _mediator.Send(command);
            if (result == null) return NotFound(new { Message = $"User with email '{command.Email}' not found." });
            return Ok(result);
        }
    }
}
