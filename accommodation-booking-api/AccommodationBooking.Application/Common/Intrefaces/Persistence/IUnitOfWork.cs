namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
