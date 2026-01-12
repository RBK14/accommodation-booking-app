using AccommodationBooking.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<ErrorOr<AuthResultDto>>;
}
