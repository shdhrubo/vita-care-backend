using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Commands
{
    public class ChangeAppointmentStatusCommandHandler : IRequestHandler<ChangeAppointmentStatusCommand, bool>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public ChangeAppointmentStatusCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> Handle(ChangeAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            var statusView = new EnumValueView
            {
                Value = request.Status,
                ViewValue = GetStatusViewValue((AppointmentStatus)request.Status)
            };
            
            return await _appointmentRepository.ChangeStatusAsync(request.Id, statusView, cancellationToken);
        }

        private string GetStatusViewValue(AppointmentStatus status)
        {
            return status switch
            {
                AppointmentStatus.Requested   => "Requested",
                AppointmentStatus.Approved    => "Approved",
                AppointmentStatus.Canceled    => "Canceled",
                AppointmentStatus.Visited     => "Visited",
                AppointmentStatus.NotVisited  => "Not Visited",
                _ => status.ToString()
            };
        }
    }
}
