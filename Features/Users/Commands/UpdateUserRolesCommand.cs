using FluentValidation;
using MediatR;

namespace vita_care.Features.Users.Commands
{
    public class UpdateUserRolesCommand : IRequest<UpsertUserResponse?>
    {
        public string Email { get; set; } = default!;
        public List<string> AddedRoles { get; set; } = new();
        public List<string> RemovedRoles { get; set; } = new();
    }

    public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
    {
        public UpdateUserRolesCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x).Must(x => x.AddedRoles.Count > 0 || x.RemovedRoles.Count > 0)
                .WithMessage("At least one role must be added or removed.");
        }
    }
}
