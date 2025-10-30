using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user, Guid profileId);
    }
}
