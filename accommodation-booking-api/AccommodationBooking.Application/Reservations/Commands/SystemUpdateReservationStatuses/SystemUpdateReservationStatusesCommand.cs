using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.SystemUpdateReservationStatuses
{
    public record SystemUpdateReservationStatusesCommand() : IRequest<Unit>;
}
