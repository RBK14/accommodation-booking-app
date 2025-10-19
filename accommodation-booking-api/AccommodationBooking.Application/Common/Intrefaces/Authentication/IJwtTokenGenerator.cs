using AccommodationBooking.Domain.Users;

namespace AccommodationBooking.Application.Common.Intrefaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);
    }
}
