using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails
{
    public record UpdatePersonalDetailsCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}