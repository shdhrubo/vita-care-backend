using MediatR;
using vita_care.Models;

namespace vita_care.Features.Doctors.Queries
{
    public class GetDoctorByIdQuery : IRequest<Doctor?>
    {
        public Guid Id { get; set; }
    }
}
