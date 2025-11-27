using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class ListingRepository(AppDbContext context) : IListingRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Listing listing)
        {
            _context.Listings.Add(listing);
        }

        public async Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Listings
                .Include(l => l.ScheduleSlots)
                .Include(l => l.Reviews)
                .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Listing>> SearchAsync(IEnumerable<IFilterable<Listing>> filters, CancellationToken cancellationToken = default)
        {
            var query = _context.Listings.AsQueryable();

            if (filters is not null)
            {
                foreach (var filter in filters)
                    query = filter.Apply(query);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public void Remove(Listing listing)
        {
            _context.Listings.Remove(listing);
        }

        public void RemoveRange(IEnumerable<Listing> listings)
        {
            if (listings.Any())
                _context.Listings.RemoveRange(listings);
        }

        public async Task<Review?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default)
        {
            return await _context.Listings
                .AsNoTracking()
                .SelectMany(l => l.Reviews)
                .FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);
        }


        public async Task<IEnumerable<Review>> SearchReviewsAsync(IEnumerable<IFilterable<Review>> filters, CancellationToken cancellationToken = default)
        {
            var query = _context.Listings
                .AsNoTracking()
                .SelectMany(l => l.Reviews);

            if (filters is not null)
            {
                foreach (var filter in filters)
                    query = filter.Apply(query);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
