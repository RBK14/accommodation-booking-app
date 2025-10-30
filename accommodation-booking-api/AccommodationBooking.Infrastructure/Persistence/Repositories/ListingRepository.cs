using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.GuestProfileAggregate;
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
