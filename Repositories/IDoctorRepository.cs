using vita_care.Models;

namespace vita_care.Repositories
{
    public interface IDoctorRepository
    {
        Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
    }
}
