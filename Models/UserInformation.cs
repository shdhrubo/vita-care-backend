using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace vita_care.Models
{
    public class UserInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;

        public List<string> Roles { get; set; } = new();
    }
}
