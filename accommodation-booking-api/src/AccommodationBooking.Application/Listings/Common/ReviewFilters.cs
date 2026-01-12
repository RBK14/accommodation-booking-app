using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ListingAggregate.Entities;

namespace AccommodationBooking.Application.Listings.Common
{
    public class ListingIdFilter(Guid? listingId) : IFilterable<Review>
    {
        private readonly Guid? _listingId = listingId;

        public IQueryable<Review> Apply(IQueryable<Review> query)
        {
            if (_listingId is not null && !string.IsNullOrWhiteSpace(_listingId.ToString()))
                query = query.Where(l => l.ListingId.Equals(_listingId));

            return query;
        }
    }

    public class GuestProfileIdFilter(Guid? guestProfileId) : IFilterable<Review>
    {
        private readonly Guid? _guestProfileId = guestProfileId;

        public IQueryable<Review> Apply(IQueryable<Review> query)
        {
            if (_guestProfileId is not null && !string.IsNullOrWhiteSpace(_guestProfileId.ToString()))
                query = query.Where(l => l.GuestProfileId.Equals(_guestProfileId));

            return query;
        }
    }
}
