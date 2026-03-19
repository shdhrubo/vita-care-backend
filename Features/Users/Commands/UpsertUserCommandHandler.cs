using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Users.Commands
{
    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, UpsertUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpsertUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpsertUserResponse> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserInformation
            {
                Email = request.Email,
                Name = request.Name
            };

            var result = await _userRepository.UpsertByUserEmailAsync(user, cancellationToken);
            
            return new UpsertUserResponse
            {
                Email = result.Email,
                Name = result.Name,
                Roles = result.Roles
            };
        }
    }
}
