using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
{
    public record UpdateEmailCommand(
        Guid UserId,
        string Email) : IRequest<ErrorOr<Unit>>;
}
