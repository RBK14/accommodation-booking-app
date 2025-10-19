using ErrorOr;
using MediatR;

namespace AccomodationBooking.Application.Users
{
    public record UpdatePersonaDetailsCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}