using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.GuestProfileAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class GuestProfileRepository : IGuestProfileRepository
    {
        private readonly List<GuestProfile> _guestProfiles = new();

        public void Add(GuestProfile guestProfile)
        {
            _guestProfiles.Add(guestProfile);
        }

        public Task<GuestProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var guestProfile = _guestProfiles.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(guestProfile);
        }

        public Task<GuestProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var guestProfile = _guestProfiles.FirstOrDefault(p => p.UserId == userId);
            return Task.FromResult(guestProfile);
        }

        public void Update(GuestProfile guestProfile)
        {
            return;
        }
        public void Remove(GuestProfile guestProfile)
        {
            return;
        }
    }
}
