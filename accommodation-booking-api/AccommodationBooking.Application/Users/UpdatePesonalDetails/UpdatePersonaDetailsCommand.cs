using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.UpdatePesonalDetails
{
    public record UpdatePersonaDetailsCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}