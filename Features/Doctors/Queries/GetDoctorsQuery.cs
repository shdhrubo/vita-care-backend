using MediatR;
using vita_care.Models;

namespace vita_care.Features.Doctors.Queries
{
    public class GetDoctorsQuery : IRequest<PaginatedResult<Doctor>>
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
