using MediatR;
using vita_care.Models;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentStatsQuery : IRequest<AppointmentStats>
    {
        public string Email { get; set; } = default!;
    }

    public class GetAppointmentStatsQueryHandler : IRequestHandler<GetAppointmentStatsQuery, AppointmentStats>
    {
        private readonly vita_care.Repositories.IAppointmentRepository _appointmentRepository;

        public GetAppointmentStatsQueryHandler(vita_care.Repositories.IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<AppointmentStats> Handle(GetAppointmentStatsQuery request, CancellationToken cancellationToken)
        {
            return await _appointmentRepository.GetStatsByEmailAsync(request.Email, cancellationToken);
        }
    }
}
