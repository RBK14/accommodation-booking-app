using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Authentication.Common
{
    public record AuthResultDTO(
        User User,
        string AccessToken);
}