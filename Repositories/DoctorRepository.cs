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

        public async Task<(List<Doctor> Items, long TotalCount)> GetPaginatedDoctorsAsync(
            string? search, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<Doctor>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchPattern = new MongoDB.Bson.BsonRegularExpression(search, "i");
                filter &= filterBuilder.Or(
                    filterBuilder.Regex(d => d.Name, searchPattern),
                    filterBuilder.Regex(d => d.Specializations, searchPattern),
                    filterBuilder.Regex(d => d.Department, searchPattern)
                );
            }

            var totalCount = await _doctorsCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var items = await _doctorsCollection.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _doctorsCollection.Find(d => d.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateDoctorAsync(Doctor doctor, CancellationToken cancellationToken)
        {
            await _doctorsCollection.ReplaceOneAsync(d => d.Id == doctor.Id, doctor, cancellationToken: cancellationToken);
        }
    }
}
