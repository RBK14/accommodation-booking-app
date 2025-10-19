using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.UpdateEmail
{
    public record UpdateEmailCommand(
        Guid Id,
        string Email) : IRequest<ErrorOr<Unit>>;
}
