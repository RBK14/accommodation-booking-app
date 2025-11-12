using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ListingAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly List<Listing> _listings = new();

        public void Add(Listing listing)
        {
            _listings.Add(listing);
        }

        public Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var listing = _listings.FirstOrDefault(l => l.Id == id);
            return Task.FromResult(listing);
        }
        public Task<IEnumerable<Listing>> SearchAsync(IEnumerable<IFilterable<Listing>> filters, CancellationToken cancellationToken = default)
        {
            var listingQuery = _listings.AsQueryable();

            if (filters is not null)
            {
                foreach (var filter in filters)
                {
                    listingQuery = filter.Apply(listingQuery);
                }
            }

            return Task.FromResult<IEnumerable<Listing>>(listingQuery.ToList());
        }

        public void Update(Listing listing)
        {
            return;
        }
        public void Remove(Listing listing)
        {
            return;
        }
    }
}
