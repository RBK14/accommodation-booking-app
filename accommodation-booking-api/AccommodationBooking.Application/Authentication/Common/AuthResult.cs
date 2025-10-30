using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Authentication.Common
{
    public record AuthResult(
        User User,
        string AccessToken);
}