using MediatR;
using vita_care.Models;

namespace vita_care.Features.Doctors.Commands
{
    public class UpdateDoctorCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int Gender { get; set; }
        public string Specializations { get; set; } = default!;
        public string Department { get; set; } = default!;
        public int[] AvailableDays { get; set; } = default!;
        public List<int> Slots { get; set; } = default!;
    }
}
