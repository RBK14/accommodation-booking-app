using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Entities;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IListingRepository
    {
        void Add(Listing listing);
        Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Listing>> SearchAsync(IEnumerable<IFilterable<Listing>> filters, CancellationToken cancellationToken = default);
        void Remove(Listing listing);
        void RemoveRange(IEnumerable<Listing> listings);
        Task<Review?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Review>> SearchReviewsAsync(IEnumerable<IFilterable<Review>> filters, CancellationToken cancellationToken = default);
    }
}
