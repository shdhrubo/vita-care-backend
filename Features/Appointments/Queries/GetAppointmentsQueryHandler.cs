using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, PaginatedResult<Appointment>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<PaginatedResult<Appointment>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _appointmentRepository.GetPaginatedAppointmentsAsync(
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
