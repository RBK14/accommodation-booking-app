using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterHost
{
    public record RegisterHostCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}