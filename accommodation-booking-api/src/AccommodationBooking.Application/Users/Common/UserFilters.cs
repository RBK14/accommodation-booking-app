using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;

namespace AccommodationBooking.Application.Users.Common
{
    public class UserRoleFilter(UserRole? role) : IFilterable<User>
    {
        private readonly UserRole? _role = role;

        public IQueryable<User> Apply(IQueryable<User> query)
        {
            if (_role is not null)
                query = query.Where(u => u.Role.Equals(_role));

            return query;
        }
    }
}
