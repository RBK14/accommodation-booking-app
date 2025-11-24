using AccommodationBooking.Domain.GuestProfileAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IGuestProfileRepository
    {
        void Add(GuestProfile guestProfile);
        Task<GuestProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GuestProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        void Remove(GuestProfile guestProfile);
    }
}
