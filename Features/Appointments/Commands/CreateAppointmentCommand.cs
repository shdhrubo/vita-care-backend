using MediatR;

namespace vita_care.Features.Appointments.Commands
{
    public class DoctorInfoInput
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = default!;
        public string DoctorEmail { get; set; } = default!;
        public string Department { get; set; } = default!;
        public int Gender { get; set; }
    }

    public class CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public DoctorInfoInput DoctorInfo { get; set; } = default!;
        public string CreatorEmail { get; set; } = default!;
        public string CreatorName { get; set; } = default!;
        public string CreatorPhone { get; set; } = default!;
        public string Date { get; set; } = default!;
        public int Slot { get; set; }
    }
}
