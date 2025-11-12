using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.Common
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}
