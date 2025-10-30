using AccommodationBooking.Domain.HostProfileAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IHostProfileRepository
    {
        void Add(HostProfile user);
        Task<HostProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<HostProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        void Update(HostProfile hostProfile);
        void Remove(HostProfile hostProfile);
    }
}
