using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.DeleteReservation
{
    public record DeleteReservationCommand(Guid ReservationId) : IRequest<ErrorOr<Unit>>;
}
