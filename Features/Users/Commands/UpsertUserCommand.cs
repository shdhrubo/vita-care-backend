using MediatR;

namespace vita_care.Features.Users.Commands
{
    public class UpsertUserResponse
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<string> Roles { get; set; } = new();
    }

    public class UpsertUserCommand : IRequest<UpsertUserResponse>
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
