using MediatR;

namespace vita_care.Features.Appointments.Commands
{
    public class ChangeAppointmentStatusCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
