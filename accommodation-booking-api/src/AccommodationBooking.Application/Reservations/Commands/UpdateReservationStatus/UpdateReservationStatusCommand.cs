using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.UpdateReservationStatus
{
    public record UpdateReservationStatusCommand(
        Guid ReservationId,
        string Status) : IRequest<ErrorOr<Unit>>;
}
