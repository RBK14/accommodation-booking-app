using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Queries.GetUser
{
    public record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>>;
}
