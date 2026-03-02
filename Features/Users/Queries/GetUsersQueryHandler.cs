using MediatR;
using vita_care.Models;
using vita_care.Repositories;

namespace vita_care.Features.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResult<UserInformation>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedResult<UserInformation>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _userRepository.GetPaginatedUsersAsync(
                request.Search, 
                request.PageNumber, 
                request.PageSize, 
                cancellationToken);

            return new PaginatedResult<UserInformation>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
