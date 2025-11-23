using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.CreateReservation
{
    public record CreateReservationCommand(
        Guid ListingId,
        Guid GuestProfileId,
        DateTime CheckIn,
        DateTime CheckOut) : IRequest<ErrorOr<Reservation>>;
}
