using AccommodationBooking.Domain.ReservationAggregate;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Queries.GetReservations
{
    public record GetReservationsQuery(
        Guid? ListingId,
        Guid? GuestProfileId,
        Guid? HostProfileId) : IRequest<IEnumerable<Reservation>>;
}
