using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Doctors.Commands
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Unit>
    {
        private readonly IDoctorRepository _doctorRepository;

        public UpdateDoctorCommandHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = new Doctor
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = new EnumValueView 
                { 
                    Value = request.Gender, 
                    ViewValue = ((GenderType)request.Gender).ToString() 
                },
                Specializations = request.Specializations,
                Department = request.Department,
                AvailableDays = request.AvailableDays,
                Slots = request.Slots.Select(s => new EnumValueView 
                { 
                    Value = s, 
                    ViewValue = GetSlotViewValue((SlotType)s) 
                }).ToList(),
                Fee = request.Fee
            };

            await _doctorRepository.UpdateDoctorAsync(doctor, cancellationToken);
            return Unit.Value;
        }

        private string GetSlotViewValue(SlotType type)
        {
            return type switch
            {
                SlotType.Morning => "10 am to 1 pm",
                SlotType.Afternoon => "2 pm to 5 pm",
                SlotType.Evening => "5 pm to 10 pm",
                _ => type.ToString()
            };
        }
    }
}
