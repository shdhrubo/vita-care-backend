using MediatR;

namespace vita_care.Features.Appointments.Commands
{
    public class UpdateAppointmentCommand : IRequest<Unit>
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
