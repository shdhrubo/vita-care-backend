using vita_care.Models;

namespace vita_care.Repositories
{
    public interface IDoctorRepository
    {
        Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
        Task<(List<Doctor> Items, long TotalCount)> GetPaginatedDoctorsAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
    }
}
