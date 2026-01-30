using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterGuest
{
    public record RegisterGuestCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}
