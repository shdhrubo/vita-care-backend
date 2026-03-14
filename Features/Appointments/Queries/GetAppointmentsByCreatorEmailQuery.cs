using MediatR;
using vita_care.Models;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentsByCreatorEmailQuery : IRequest<PaginatedResult<Appointment>>
    {
        public string CreatorEmail { get; set; } = default!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
