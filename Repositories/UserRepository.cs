using MongoDB.Driver;
using vita_care.Models;

namespace vita_care.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserInformation> _usersCollection;

        public UserRepository(IMongoClient client, MongoDbSettings settings)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _usersCollection = database.GetCollection<UserInformation>("UsersInformation");
        }

        public async Task<(List<UserInformation> Items, long TotalCount)> GetPaginatedUsersAsync(
            string? search, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<UserInformation>.Filter;
            var filter = filterBuilder.Empty;

            // Apply Search filter (Name or Email)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchPattern = new MongoDB.Bson.BsonRegularExpression(search, "i");
                filter &= filterBuilder.Or(
                    filterBuilder.Regex(u => u.Name, searchPattern),
                    filterBuilder.Regex(u => u.Email, searchPattern)
                );
            }

            // Get total count
            var totalCount = await _usersCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            // Fetch paginated data
            var items = await _usersCollection.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<UserInformation> UpsertByUserEmailAsync(UserInformation user, CancellationToken cancellationToken)
        {
            var filter = Builders<UserInformation>.Filter.Eq(u => u.Email, user.Email);
            
            var update = Builders<UserInformation>.Update
                .Set(u => u.Name, user.Name)
                .SetOnInsert(u => u.Id, Guid.NewGuid())
                .SetOnInsert(u => u.Email, user.Email)
                .SetOnInsert(u => u.Roles, new List<string> { "patient" });

            var options = new FindOneAndUpdateOptions<UserInformation>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            return await _usersCollection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
        }

        public async Task<UserInformation?> UpdateUserRolesAsync(string email, List<string> addedRoles, List<string> removedRoles, CancellationToken cancellationToken)
        {
            var filter = Builders<UserInformation>.Filter.Eq(u => u.Email, email);

            var updateDefinitions = new List<UpdateDefinition<UserInformation>>();

            if (addedRoles.Count > 0)
                updateDefinitions.Add(Builders<UserInformation>.Update.AddToSetEach(u => u.Roles, addedRoles));

            if (removedRoles.Count > 0)
                updateDefinitions.Add(Builders<UserInformation>.Update.PullAll(u => u.Roles, removedRoles));

            if (updateDefinitions.Count == 0)
                return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            var combinedUpdate = Builders<UserInformation>.Update.Combine(updateDefinitions);

            var options = new FindOneAndUpdateOptions<UserInformation>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _usersCollection.FindOneAndUpdateAsync(filter, combinedUpdate, options, cancellationToken);
        }

        public async Task<UserInformation?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var filter = Builders<UserInformation>.Filter.Eq(u => u.Email, email);
            return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
