using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Common.Interfaces.Authentication
{
    /// <summary>
    /// Interface for generating JWT tokens.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        string GenerateAccessToken(User user, Guid profileId);
    }
}
