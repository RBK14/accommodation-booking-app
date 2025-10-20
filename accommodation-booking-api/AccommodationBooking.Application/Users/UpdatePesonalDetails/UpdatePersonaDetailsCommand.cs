using ErrorOr;
using MediatR;

<<<<<<<< HEAD:accommodation-booking-api/AccommodationBooking.Application/Users/UpdatePesonalDetails/UpdatePersonaDetailsCommand.cs
namespace AccommodationBooking.Application.Users.UpdatePesonalDetails
========
namespace AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails
>>>>>>>> 19e883eb578dc42fcd4c337d8850517a9f15d882:accommodation-booking-api/AccommodationBooking.Application/Users/Commands/UpdatePesonalDetails/UpdatePersonaDetailsCommand.cs
{
    public record UpdatePersonaDetailsCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Phone) : IRequest<ErrorOr<Unit>>;
}