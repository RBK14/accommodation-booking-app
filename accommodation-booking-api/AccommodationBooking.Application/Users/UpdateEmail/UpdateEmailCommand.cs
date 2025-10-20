using ErrorOr;
using MediatR;

<<<<<<<< HEAD:accommodation-booking-api/AccommodationBooking.Application/Users/UpdateEmail/UpdateEmailCommand.cs
namespace AccommodationBooking.Application.Users.UpdateEmail
========
namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
>>>>>>>> 19e883eb578dc42fcd4c337d8850517a9f15d882:accommodation-booking-api/AccommodationBooking.Application/Authentication/Commands/UpdateEmail/UpdateEmailCommand.cs
{
    public record UpdateEmailCommand(
        Guid UserId,
        string Email) : IRequest<ErrorOr<Unit>>;
}
