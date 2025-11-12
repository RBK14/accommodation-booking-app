using AccommodationBooking.Application.Common.Intrefaces.Persistence;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(
        IUserRepository users,
        IGuestProfileRepository guestProfiles,
        IHostProfileRepository hostProfiles,
        IListingRepository listings,
        IReservationRepository reservations) : IUnitOfWork
    {
        public IUserRepository Users { get; } = users;
        public IGuestProfileRepository GuestProfiles { get; } = guestProfiles;
        public IHostProfileRepository HostProfiles { get; } = hostProfiles;
        public IListingRepository Listings { get; } = listings;
        public IReservationRepository Reservations { get; } = reservations;

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
