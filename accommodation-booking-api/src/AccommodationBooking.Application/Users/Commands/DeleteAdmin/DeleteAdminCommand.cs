using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteAdmin
{
    public record DeleteAdminCommand(Guid UserId) : IRequest<ErrorOr<Unit>>;
}
