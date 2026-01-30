using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Queries.GetUser
{
    /// <summary>
    /// Query to get a user by ID.
    /// </summary>
    public record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>>;
}
