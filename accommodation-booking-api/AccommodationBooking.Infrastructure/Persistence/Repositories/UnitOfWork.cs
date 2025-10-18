using AccommodationBooking.Application.Common.Intrefaces.Persistence;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(IUserRepository users) : IUnitOfWork
    {
        public IUserRepository Users { get; } = users;

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
