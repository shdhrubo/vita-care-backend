using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace vita_care.Models
{
    public enum Gender
    {
        Male,
        Female,
        Others
    }

    public enum SlotType
    {
        Morning = 1,    // 10 am to 1 pm
        Afternoon = 2,  // 2 pm to 5 pm
        Evening = 3     // 5 pm to 10 pm
    }

    public class Doctor
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        
        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }
        
        public string Specializations { get; set; } = default!;
        public string Department { get; set; } = default!;
        
        public int[] AvailableDays { get; set; } = new int[7]; // [Sun, Mon, Tue, Wed, Thu, Fri, Sat]
        
        public List<SlotType> Slots { get; set; } = new();
    }
}
