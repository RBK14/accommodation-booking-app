using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.UpdateEmail
{
    public record UpdateEmailCommand(
        Guid UserId,
        string Email) : IRequest<ErrorOr<Unit>>;
}
