using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Users.Commands
{
    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public UpsertUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserInformation
            {
                Email = request.Email,
                Name = request.Name
            };

            await _userRepository.UpsertByUserEmailAsync(user, cancellationToken);
            
            return Unit.Value;
        }
    }
}
