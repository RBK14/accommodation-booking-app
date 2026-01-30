using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementation of Unit of Work pattern for managing database transactions.
    /// </summary>
    public class UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IGuestProfileRepository guestProfiles,
        IHostProfileRepository hostProfiles,
        IListingRepository listings,
        IReservationRepository reservations) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; } = users;
        public IGuestProfileRepository GuestProfiles { get; } = guestProfiles;
        public IHostProfileRepository HostProfiles { get; } = hostProfiles;
        public IListingRepository Listings { get; } = listings;
        public IReservationRepository Reservations { get; } = reservations;

        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
                _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
                await _transaction.CommitAsync(cancellationToken);

            return result;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }
}
