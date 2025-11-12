namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }
        IGuestProfileRepository GuestProfiles { get; }
        IHostProfileRepository HostProfiles { get; }
        IListingRepository Listings { get; }
        IReservationRepository Reservations { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
