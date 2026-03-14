using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentsByCreatorEmailQueryHandler : IRequestHandler<GetAppointmentsByCreatorEmailQuery, PaginatedResult<Appointment>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentsByCreatorEmailQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<PaginatedResult<Appointment>> Handle(GetAppointmentsByCreatorEmailQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _appointmentRepository.GetPaginatedByCreatorEmailAsync(
                request.CreatorEmail,
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
