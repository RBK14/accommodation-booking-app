using AccommodationBooking.Domain.GuestProfileAggregate;

namespace AccommodationBooking.Application.Common.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface for guest profile entities.
    /// </summary>
    public interface IGuestProfileRepository
    {
        void Add(GuestProfile guestProfile);
        Task<GuestProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GuestProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        void Remove(GuestProfile guestProfile);
    }
}
