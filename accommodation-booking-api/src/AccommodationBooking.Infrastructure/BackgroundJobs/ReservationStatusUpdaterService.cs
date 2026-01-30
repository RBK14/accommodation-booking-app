using AccommodationBooking.Application.Reservations.Commands.SystemUpdateReservationStatuses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AccommodationBooking.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Background service that automatically updates reservation statuses based on check-in/check-out dates.
    /// </summary>
    public class ReservationStatusUpdaterService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ReservationStatusUpdaterService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<ReservationStatusUpdaterService> _logger = logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            _logger.LogInformation("Starting reservation status update cycle...");

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _serviceScopeFactory.CreateAsyncScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

                    await mediator.Send(new SystemUpdateReservationStatusesCommand(), stoppingToken);

                    _logger.LogInformation("Reservation status update cycle finished.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Critical error in Reservation Background Service.");
                }
            }
        }
    }
}
