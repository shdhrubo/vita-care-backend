using MediatR;
using vita_care.Models;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAllAppointmentStatsQuery : IRequest<AppointmentStats>
    {
    }

    public class GetAllAppointmentStatsQueryHandler : IRequestHandler<GetAllAppointmentStatsQuery, AppointmentStats>
    {
        private readonly vita_care.Repositories.IAppointmentRepository _appointmentRepository;

        public GetAllAppointmentStatsQueryHandler(vita_care.Repositories.IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<AppointmentStats> Handle(GetAllAppointmentStatsQuery request, CancellationToken cancellationToken)
        {
            return await _appointmentRepository.GetAllStatsAsync(cancellationToken);
        }
    }
}
