using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Users;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void Add(User user)
        {
            _users.Add(user);
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }
        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public void Update(User user)
        {
            return;
        }

        public void Delete(User user)
        {
            return;
        }
    }
}
