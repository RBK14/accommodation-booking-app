using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Users.Common;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MediatR;

namespace AccommodationBooking.Application.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var role = UserRoleExtensions.TryParseRole(query.UserRole, out var parsedRole)
                ? parsedRole
                : (UserRole?)null;

            var filters = new List<IFilterable<User>>
            {
                new UserRoleFilter(role)
            };

            return await _unitOfWork.Users.SearchAsync(filters, cancellationToken);
        }
    }
}
