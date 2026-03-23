using MongoDB.Driver;
using vita_care.Models;

namespace vita_care.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoCollection<Appointment> _appointmentsCollection;

        public AppointmentRepository(IMongoClient client, MongoDbSettings settings)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _appointmentsCollection = database.GetCollection<Appointment>("AppointmentsTable");
        }

        public async Task CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            await _appointmentsCollection.InsertOneAsync(appointment, cancellationToken: cancellationToken);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            await _appointmentsCollection.ReplaceOneAsync(a => a.Id == appointment.Id, appointment, cancellationToken: cancellationToken);
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _appointmentsCollection.DeleteOneAsync(a => a.Id == id, cancellationToken: cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<(List<Appointment> Items, long TotalCount)> GetPaginatedAppointmentsAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<Appointment>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchPattern = new MongoDB.Bson.BsonRegularExpression(search, "i");
                filter &= filterBuilder.Or(
                    filterBuilder.Regex(a => a.DoctorInfo.DoctorName, searchPattern),
                    filterBuilder.Regex(a => a.DoctorInfo.Department, searchPattern),
                    filterBuilder.Regex(a => a.DoctorInfo.Specializations, searchPattern),
                    filterBuilder.Regex(a => a.CreatorName, searchPattern)
                );
            }

            var totalCount = await _appointmentsCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var items = await _appointmentsCollection.Find(filter)
                .SortByDescending(a => a.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<Appointment> Items, long TotalCount)> GetPaginatedByEmailAsync(string email, string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<Appointment>.Filter;
            
            // Search either the CreatorEmail or the DoctorEmail
            var emailFilter = filterBuilder.Or(
                filterBuilder.Eq(a => a.CreatorEmail, email),
                filterBuilder.Eq(a => a.DoctorInfo.DoctorEmail, email)
            );

            var filter = emailFilter;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchPattern = new MongoDB.Bson.BsonRegularExpression(search, "i");
                var searchFilter = filterBuilder.Or(
                    filterBuilder.Regex(a => a.DoctorInfo.DoctorName, searchPattern),
                    filterBuilder.Regex(a => a.DoctorInfo.Department, searchPattern),
                    filterBuilder.Regex(a => a.DoctorInfo.Specializations, searchPattern),
                    filterBuilder.Regex(a => a.CreatorName, searchPattern)
                );
                filter &= searchFilter;
            }

            var totalCount = await _appointmentsCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var items = await _appointmentsCollection.Find(filter)
                .SortByDescending(a => a.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<bool> ChangeStatusAsync(Guid id, EnumValueView status, CancellationToken cancellationToken)
        {
            var update = Builders<Appointment>.Update.Set(a => a.Status, status);
            var result = await _appointmentsCollection.UpdateOneAsync(a => a.Id == id, update, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appointmentsCollection.Find(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
