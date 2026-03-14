using FluentValidation;
using vita_care.Models;

namespace vita_care.Features.Appointments.Commands
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.CreatorEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.CreatorName).NotEmpty();
            RuleFor(x => x.CreatorPhone).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Slot).Must(val => Enum.IsDefined(typeof(SlotType), val)).WithMessage("Invalid Slot.");
            
            RuleFor(x => x.DoctorInfo).NotNull();
            When(x => x.DoctorInfo != null, () => {
                RuleFor(x => x.DoctorInfo.DoctorId).NotEmpty();
                RuleFor(x => x.DoctorInfo.DoctorName).NotEmpty();
                RuleFor(x => x.DoctorInfo.DoctorEmail).NotEmpty().EmailAddress();
                RuleFor(x => x.DoctorInfo.Department).NotEmpty();
                RuleFor(x => x.DoctorInfo.Gender).Must(val => Enum.IsDefined(typeof(GenderType), val)).WithMessage("Invalid Gender.");
            });
        }
    }

    public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CreatorEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.CreatorName).NotEmpty();
            RuleFor(x => x.CreatorPhone).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Slot).Must(val => Enum.IsDefined(typeof(SlotType), val)).WithMessage("Invalid Slot.");
            
            RuleFor(x => x.DoctorInfo).NotNull();
            When(x => x.DoctorInfo != null, () => {
                RuleFor(x => x.DoctorInfo.DoctorId).NotEmpty();
                RuleFor(x => x.DoctorInfo.DoctorName).NotEmpty();
                RuleFor(x => x.DoctorInfo.DoctorEmail).NotEmpty().EmailAddress();
                RuleFor(x => x.DoctorInfo.Department).NotEmpty();
                RuleFor(x => x.DoctorInfo.Gender).Must(val => Enum.IsDefined(typeof(GenderType), val)).WithMessage("Invalid Gender.");
            });
        }
    }

    public class ChangeAppointmentStatusCommandValidator : AbstractValidator<ChangeAppointmentStatusCommand>
    {
        public ChangeAppointmentStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Status).Must(val => Enum.IsDefined(typeof(AppointmentStatus), val)).WithMessage("Invalid Status.");
        }
    }

    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
