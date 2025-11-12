using AccommodationBooking.Domain.UserAggregate;
using MediatR;

namespace AccommodationBooking.Application.Users.Queries.GetUsers
{
    public record GetUsersQuery(string UserRole) : IRequest<IEnumerable<User>>;
}
