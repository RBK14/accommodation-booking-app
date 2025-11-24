using AccommodationBooking.Domain.UserAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> SearchAsync(IEnumerable<IFilterable<User>> filters, CancellationToken cancellationToken = default);
        void Remove(User user);
    }
}
