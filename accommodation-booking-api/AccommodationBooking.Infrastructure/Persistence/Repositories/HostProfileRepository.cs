using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.HostProfileAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class HostProfileRepository : IHostProfileRepository
    {
        private readonly List<HostProfile> _hostProfiles = new();

        public void Add(HostProfile hostProfile)
        {
            _hostProfiles.Add(hostProfile);
        }

        public Task<HostProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var hostProfile = _hostProfiles.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(hostProfile);
        }

        public Task<HostProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var hostProfile = _hostProfiles.FirstOrDefault(p => p.UserId == userId);
            return Task.FromResult(hostProfile);
        }

        public void Update(HostProfile hostProfile)
        {
            return;
        }
        public void Remove(HostProfile hostProfile)
        {
            return;
        }
    }
}
