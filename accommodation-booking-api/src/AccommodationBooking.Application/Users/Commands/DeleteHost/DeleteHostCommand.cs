using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteHost
{
    public record DeleteHostCommand(Guid UserId) : IRequest<ErrorOr<Unit>>;
}
