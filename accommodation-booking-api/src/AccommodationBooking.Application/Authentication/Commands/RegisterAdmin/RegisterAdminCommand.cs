using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterAdmin
{
    public record RegisterAdminCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}
