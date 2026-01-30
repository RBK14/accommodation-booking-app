using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.HostProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class HostProfileRepository(AppDbContext context) : IHostProfileRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(HostProfile hostProfile)
        {
            _context.HostProfiles.Add(hostProfile);
        }

        public async Task<HostProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.HostProfiles
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<HostProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.HostProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        }

        public void Remove(HostProfile hostProfile)
        {
            _context.HostProfiles.Remove(hostProfile);
        }
    }
}
