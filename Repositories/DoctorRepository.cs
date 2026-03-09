using MongoDB.Driver;
using vita_care.Models;

namespace vita_care.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IMongoCollection<Doctor> _doctorsCollection;

        public DoctorRepository(IMongoClient client, MongoDbSettings settings)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _doctorsCollection = database.GetCollection<Doctor>("DoctorsTable");
        }

        public async Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken)
        {
            await _doctorsCollection.InsertOneAsync(doctor, cancellationToken: cancellationToken);
        }
    }
}
