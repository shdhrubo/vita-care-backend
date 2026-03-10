using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Doctors.Queries
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Doctor?>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorByIdQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Doctor?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _doctorRepository.GetDoctorByIdAsync(request.Id, cancellationToken);
        }
    }
}
