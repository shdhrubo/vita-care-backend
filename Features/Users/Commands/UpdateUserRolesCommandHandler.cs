using MediatR;
using vita_care.Repositories;

namespace vita_care.Features.Users.Commands
{
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, UpsertUserResponse?>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserRolesCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpsertUserResponse?> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.UpdateUserRolesAsync(
                request.Email,
                request.AddedRoles,
                request.RemovedRoles,
                cancellationToken);

            if (result == null) return null;

            return new UpsertUserResponse
            {
                Email = result.Email,
                Name = result.Name,
                Roles = result.Roles
            };
        }
    }
}
