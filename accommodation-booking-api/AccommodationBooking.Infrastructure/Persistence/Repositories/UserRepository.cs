using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void Add(User user)
        {
            _users.Add(user);
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }
        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public void Update(User user)
        {
            return;
        }

        public void Remove(User user)
        {
            _users.Remove(user);
        }
    }
}
