using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ListingAggregate;

namespace AccommodationBooking.Application.Listings.Common
{
    public class HostProfileIdFilter(Guid? hostProfileId) : IFilterable<Listing>
    {
        private readonly Guid? _hostProfileId = hostProfileId;

        public IQueryable<Listing> Apply(IQueryable<Listing> query)
        {
            if (_hostProfileId is not null && !string.IsNullOrWhiteSpace(_hostProfileId.ToString()))
                query = query.Where(l => l.HostProfileId.Equals(_hostProfileId));

            return query;
        }
    }

    public class ReviewIdFilter(Guid? reviewId) : IFilterable<Listing>
    {
        private readonly Guid? _reviewId = reviewId;

        public IQueryable<Listing> Apply(IQueryable<Listing> query)
        {
            if (_reviewId is not null && !string.IsNullOrWhiteSpace(_reviewId.ToString()))
                query = query.Where(l => l.Reviews.Any(r => r.Id == _reviewId));

            return query;
        }
    }

    public class ListingIdsFilter(IEnumerable<Guid>? listingIds) : IFilterable<Listing>
    {
        private readonly IEnumerable<Guid>? _listingIds = listingIds;

        public IQueryable<Listing> Apply(IQueryable<Listing> query)
        {
            if (_listingIds is not null && _listingIds.Any())
            {
                query = query.Where(l => _listingIds.Contains(l.Id));
            }

            return query;
        }
    }
}
