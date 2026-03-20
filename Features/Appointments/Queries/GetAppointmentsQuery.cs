using MediatR;
using vita_care.Models;

namespace vita_care.Features.Appointments.Queries
{
    public class GetAppointmentsQuery : IRequest<PaginatedResult<Appointment>>
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
