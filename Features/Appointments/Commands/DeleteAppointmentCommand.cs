using MediatR;

namespace vita_care.Features.Appointments.Commands
{
    public class DeleteAppointmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
