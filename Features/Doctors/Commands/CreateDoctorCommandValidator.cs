using FluentValidation;
using vita_care.Repositories;
using vita_care.Models;

namespace vita_care.Features.Doctors.Commands
{
    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber).NotEmpty();
            
            RuleFor(x => x.Gender).Must(val => Enum.IsDefined(typeof(GenderType), val)).WithMessage("Invalid Gender.");
            
            RuleFor(x => x.Specializations).NotEmpty();
            
            RuleFor(x => x.Department).NotEmpty();
            
            RuleFor(x => x.AvailableDays)
                .NotNull()
                .Must(x => x.Length == 7)
                .WithMessage("AvailableDays must contain 7 elements (0 or 1).")
                .Must(x => x.All(val => val == 0 || val == 1))
                .WithMessage("AvailableDays elements must be only 0 or 1.");

            RuleFor(x => x.Slots)
                .NotEmpty()
                .Must(x => x.All(val => val >= 1 && val <= 3))
                .WithMessage("Slots must be between 1 and 3.");
        }
    }
}
