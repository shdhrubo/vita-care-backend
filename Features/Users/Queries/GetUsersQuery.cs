using MediatR;
using vita_care.Models;

namespace vita_care.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<PaginatedResult<UserInformation>>
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
