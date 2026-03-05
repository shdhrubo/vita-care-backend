using MediatR;

namespace vita_care.Features.Users.Commands
{
    public class UpsertUserCommand : IRequest<Unit>
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
