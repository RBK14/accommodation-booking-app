using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdatePassword
{
    public record UpdatePasswordCommand(
        Guid UserId,
        string Password,
        string NewPassword) : IRequest<ErrorOr<Unit>>;
}
