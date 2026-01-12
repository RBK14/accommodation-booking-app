using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Reservations.Common;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccommodationBooking.Application.Reservations.Commands.SystemUpdateReservationStatuses
{
    public class SystemUpdateReservationStatusesCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<SystemUpdateReservationStatusesCommandHandler> logger) : IRequestHandler<SystemUpdateReservationStatusesCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<SystemUpdateReservationStatusesCommandHandler> _logger = logger;

        public async Task<Unit> Handle(SystemUpdateReservationStatusesCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing automatic reservation status updates...");

            var now = DateTime.UtcNow;

            try
            {
                var startFilters = new List<IFilterable<Reservation>>
                {
                    new ReservationStatusFilter(ReservationStatus.Accepted),
                    new CheckInFilter(now)
                };

                var reservationsToStart = await _unitOfWork.Reservations.SearchAsync(startFilters, cancellationToken);

                foreach (var reservation in reservationsToStart)
                {
                    reservation.MarkAsInProgress();
                }

                var completeFilters = new List<IFilterable<Reservation>>
                {
                    new ReservationStatusFilter(ReservationStatus.InProgress),
                    new CheckOutFilter(now)
                };

                var reservationsToComplete = await _unitOfWork.Reservations.SearchAsync(completeFilters, cancellationToken);

                foreach (var reservation in reservationsToComplete)
                {
                    reservation.MarkAsCompleted();
                }

                var startCount = reservationsToStart.Count();
                var completeCount = reservationsToComplete.Count();

                if (startCount > 0 || completeCount > 0)
                {
                    await _unitOfWork.CommitAsync(cancellationToken);

                    _logger.LogInformation(
                        "Reservation statuses updated successfully. Started: {StartCount}, Completed: {CompleteCount}.",
                        startCount,
                        completeCount);
                }
                else
                {
                    _logger.LogInformation("No reservations required status updates.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update system reservation statuses.");
            }

            return Unit.Value;
        }
    }
}
