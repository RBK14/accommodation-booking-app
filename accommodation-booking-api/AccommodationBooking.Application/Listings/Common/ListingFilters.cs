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
}
