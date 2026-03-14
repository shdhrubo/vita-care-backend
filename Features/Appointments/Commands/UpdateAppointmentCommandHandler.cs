using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Commands
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(request.Id, cancellationToken);
            if (existingAppointment == null)
            {
                throw new KeyNotFoundException("Appointment not found");
            }

            existingAppointment.DoctorInfo = new DoctorInfo
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
            };
            existingAppointment.CreatorEmail = request.CreatorEmail;
            existingAppointment.CreatorName = request.CreatorName;
            existingAppointment.CreatorPhone = request.CreatorPhone;
            existingAppointment.Date = request.Date;
            existingAppointment.Slot = new EnumValueView
            {
                Value = request.Slot,
                ViewValue = GetSlotViewValue((SlotType)request.Slot)
            };

            await _appointmentRepository.UpdateAppointmentAsync(existingAppointment, cancellationToken);
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
