using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteGuest
{
    public record DeleteGuestCommand(Guid UserId) : IRequest<ErrorOr<Unit>>;
}
