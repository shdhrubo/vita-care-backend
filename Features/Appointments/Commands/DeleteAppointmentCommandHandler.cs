using MediatR;
using vita_care.Repositories;

namespace vita_care.Features.Appointments.Commands
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, bool>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public DeleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            return await _appointmentRepository.DeleteAppointmentAsync(request.Id, cancellationToken);
        }
    }
}
