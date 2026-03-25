using vita_care.Models;

namespace vita_care.Repositories
{
    public interface IUserRepository
    {
        Task<(List<UserInformation> Items, long TotalCount)> GetPaginatedUsersAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<UserInformation> UpsertByUserEmailAsync(UserInformation user, CancellationToken cancellationToken);
        Task<UserInformation?> UpdateUserRolesAsync(string email, List<string> addedRoles, List<string> removedRoles, CancellationToken cancellationToken);
        Task<UserInformation?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
