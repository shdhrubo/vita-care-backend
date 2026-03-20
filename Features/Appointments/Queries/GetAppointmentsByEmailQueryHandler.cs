using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentsByEmailQueryHandler : IRequestHandler<GetAppointmentsByEmailQuery, PaginatedResult<Appointment>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentsByEmailQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<PaginatedResult<Appointment>> Handle(GetAppointmentsByEmailQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _appointmentRepository.GetPaginatedByEmailAsync(
                request.Email,
                request.Search,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return new PaginatedResult<Appointment>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
