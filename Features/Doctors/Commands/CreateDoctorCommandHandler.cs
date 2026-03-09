using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Doctors.Commands
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
        private readonly IDoctorRepository _doctorRepository;

        public CreateDoctorCommandHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                Specializations = request.Specializations,
                Department = request.Department,
                AvailableDays = request.AvailableDays,
                Slots = request.Slots.Select(s => (SlotType)s).ToList()
            };

            await _doctorRepository.CreateDoctorAsync(doctor, cancellationToken);
            return doctor.Id;
        }
    }
}
