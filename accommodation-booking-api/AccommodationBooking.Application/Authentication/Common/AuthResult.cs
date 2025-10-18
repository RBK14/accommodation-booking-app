using AccommodationBooking.Domain.Users;

namespace AccommodationBooking.Application.Authentication.Common
{
    public record AuthResult(
        User User,
        string Token);
}