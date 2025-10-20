using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdatePassword
{
    public record UpdatePasswordCommand(
        Guid Id,
        string Password,
        string NewPassword) : IRequest<ErrorOr<Unit>>;
}
