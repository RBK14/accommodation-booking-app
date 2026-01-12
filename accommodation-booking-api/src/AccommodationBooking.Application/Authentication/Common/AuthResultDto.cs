using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Authentication.Common
{
    public record AuthResultDto(
        User User,
        string AccessToken);
}