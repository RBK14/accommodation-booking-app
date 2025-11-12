using AccommodationBooking.Domain.ListingAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IListingRepository
    {
        void Add(Listing listing);
        Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Listing>> SearchAsync(IEnumerable<IFilterable<Listing>> filters, CancellationToken cancellationToken = default);
        void Update(Listing listing);
        void Remove(Listing listing);
    }
}
