using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Doctors.Queries
{
    public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, PaginatedResult<Doctor>>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorsQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<PaginatedResult<Doctor>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _doctorRepository.GetPaginatedDoctorsAsync(
                request.Search,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return new PaginatedResult<Doctor>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
