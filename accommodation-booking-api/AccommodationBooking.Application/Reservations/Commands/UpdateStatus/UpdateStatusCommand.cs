using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.UpdateStatus
{
    public record UpdateStatusCommand(Guid ReservationId, string Status) : IRequest<ErrorOr<Unit>>;
}
