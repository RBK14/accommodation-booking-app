using AccommodationBooking.Domain.HostProfileAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    /// <summary>
    /// Repository interface for host profile entities.
    /// </summary>
    public interface IHostProfileRepository
    {
        void Add(HostProfile hostProfile);
        Task<HostProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<HostProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        void Remove(HostProfile hostProfile);
    }
}
