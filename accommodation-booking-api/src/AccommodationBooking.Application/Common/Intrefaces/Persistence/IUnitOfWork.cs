namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    /// <summary>
    /// Unit of Work pattern interface for managing database transactions.
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }
        IGuestProfileRepository GuestProfiles { get; }
        IHostProfileRepository HostProfiles { get; }
        IListingRepository Listings { get; }
        IReservationRepository Reservations { get; }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits all changes and the current transaction.
        /// </summary>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
