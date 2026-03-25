using MediatR;

namespace vita_care.Features.Doctors.Commands
{
    public class DeleteDoctorCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, bool>
    {
        private readonly vita_care.Repositories.IDoctorRepository _doctorRepository;

        public DeleteDoctorCommandHandler(vita_care.Repositories.IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            return await _doctorRepository.DeleteDoctorAsync(request.Id, cancellationToken);
        }
    }
}
