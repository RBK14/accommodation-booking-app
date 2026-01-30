using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.UpdatePersonalDetails
{
    /// <summary>
    /// Command to update user's personal details.
    /// </summary>
    public record UpdatePersonalDetailsCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}
