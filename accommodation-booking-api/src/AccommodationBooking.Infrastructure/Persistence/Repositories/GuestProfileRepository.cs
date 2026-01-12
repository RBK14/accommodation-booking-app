using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.GuestProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class GuestProfileRepository(AppDbContext context) : IGuestProfileRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(GuestProfile guestProfile)
        {
            _context.GuestProfiles.Add(guestProfile);
        }

        public async Task<GuestProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.GuestProfiles
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<GuestProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.GuestProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        }

        public void Remove(GuestProfile guestProfile)
        {
            _context.GuestProfiles.Remove(guestProfile);
        }
    }
}
