using vita_care.Models;

namespace vita_care.Repositories
{
    public interface IUserRepository
    {
        Task<(List<UserInformation> Items, long TotalCount)> GetPaginatedUsersAsync(string? search, string? role, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
