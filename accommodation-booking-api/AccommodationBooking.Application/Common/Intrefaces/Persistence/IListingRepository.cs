using AccommodationBooking.Domain.ListingAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IListingRepository
    {
        void Add(Listing listing);
        Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Update(Listing listing);
        void Remove(Listing listing);
    }
}
