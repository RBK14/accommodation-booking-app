using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<User>> SearchAsync(IEnumerable<IFilterable<User>> filters, CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsQueryable();

            if (filters is not null)
            {
                foreach (var filter in filters)
                    query = filter.Apply(query);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
