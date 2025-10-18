using AccommodationBooking.Domain.Users;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        void Update(User user);
        void Delete(User user);
    }
}
