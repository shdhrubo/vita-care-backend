using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace vita_care.Models
{
    public enum AppointmentStatus
    {
        Requested = 1,
        Approved = 2,
        Canceled = 3,
        Visited = 4,
        NotVisited = 5
    }

    public class DoctorInfo
    {
        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = default!;
        public string DoctorEmail { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string Specializations { get; set; } = default!;
        public EnumValueView Gender { get; set; } = default!;
    }

    public class Appointment
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        public Guid Id { get; set; }
        
        public DoctorInfo DoctorInfo { get; set; } = default!;
        
        public string CreatorEmail { get; set; } = default!;
        public string CreatorName { get; set; } = default!;
        public string CreatorPhone { get; set; } = default!;
        
        public string Date { get; set; } = default!; // Ex: "2024-12-31"
        
        public EnumValueView Slot { get; set; } = default!;
        
        public EnumValueView Status { get; set; } = default!;
    }
}
