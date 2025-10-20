using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
{
    public record UpdateEmailCommand(
        Guid Id,
        string Email) : IRequest<ErrorOr<Unit>>;
}
