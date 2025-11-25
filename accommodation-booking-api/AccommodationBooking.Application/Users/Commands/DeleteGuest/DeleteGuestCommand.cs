using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteGuest
{
    public record DeleteGuestCommand(Guid UserId, Guid GuestProfileId) : IRequest<ErrorOr<Unit>>;
}
