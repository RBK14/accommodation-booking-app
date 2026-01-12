using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Queries.GetReservation
{
    public record GetReservationQuery(Guid ReservationId) : IRequest<ErrorOr<Reservation>>;
}
