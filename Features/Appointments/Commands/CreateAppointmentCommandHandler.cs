using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Commands
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorInfo = new DoctorInfo
                {
                    DoctorId = request.DoctorInfo.DoctorId,
                    DoctorName = request.DoctorInfo.DoctorName,
                    DoctorEmail = request.DoctorInfo.DoctorEmail,
                    Department = request.DoctorInfo.Department,
                    Gender = new EnumValueView
                    {
                        Value = request.DoctorInfo.Gender,
                        ViewValue = ((GenderType)request.DoctorInfo.Gender).ToString()
                    }
                },
                CreatorEmail = request.CreatorEmail,
                CreatorName = request.CreatorName,
                CreatorPhone = request.CreatorPhone,
                Date = request.Date,
                Slot = new EnumValueView
                {
                    Value = request.Slot,
                    ViewValue = GetSlotViewValue((SlotType)request.Slot)
                },
                Status = new EnumValueView
                {
                    Value = (int)AppointmentStatus.Requested,
                    ViewValue = AppointmentStatus.Requested.ToString()
                }
            };

            await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
            return appointment.Id;
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
