using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.UpdateEmail
{
    public record UpdateEmailCommand(
        Guid Id,
        string Email) : IRequest<ErrorOr<Unit>>;
}
