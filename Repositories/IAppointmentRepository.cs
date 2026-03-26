using vita_care.Models;

namespace vita_care.Repositories
{
    public interface IAppointmentRepository
    {
        Task CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<bool> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken);
        Task<(List<Appointment> Items, long TotalCount)> GetPaginatedAppointmentsAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<(List<Appointment> Items, long TotalCount)> GetPaginatedByEmailAsync(string email, string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<bool> ChangeStatusAsync(Guid id, EnumValueView status, CancellationToken cancellationToken);
        Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<AppointmentStats> GetStatsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<AppointmentStats> GetAllStatsAsync(CancellationToken cancellationToken);
    }
}
