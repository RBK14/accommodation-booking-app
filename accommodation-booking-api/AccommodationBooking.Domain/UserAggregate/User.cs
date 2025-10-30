using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.UserAggregate.Enums;

namespace AccommodationBooking.Domain.UserAggregate
{
    public class User : AggregateRoot<Guid>
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone {  get; private set; }

        public UserRole Role { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private User(
            Guid id,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            string phone,
            UserRole role,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Role = role;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static User CreateGuest(string email, string passwordHash, string firstName, string lastName, string phone)
        {
            var id = Guid.NewGuid();

            return new User(
                id,
                email,
                passwordHash,
                firstName,
                lastName,
                phone,
                UserRole.Guest,
                DateTime.UtcNow,
                DateTime.UtcNow); 
        }

        public static User CreateHost(string email, string passwordHash, string firstName, string lastName, string phone)
        {
            var id = Guid.NewGuid();

            return new User(
                id,
                email,
                passwordHash,
                firstName,
                lastName,
                phone,
                UserRole.Host,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public static User CreateAdmin(string email, string passwordHash, string firstName, string lastName, string phone)
        {
            return new User(
                Guid.NewGuid(),
                email,
                passwordHash,
                firstName,
                lastName,
                phone,
                UserRole.Admin,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateEmail(string email)
        {
            if (Email == email)
                return;

            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            if (PasswordHash == passwordHash)
                return;

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePersonalDetails(
            string firstName,
            string lastName,
            string phone)
        {
            if (FirstName != firstName)
            {
                FirstName = firstName;
                UpdatedAt = DateTime.UtcNow;
            }

            if (LastName != lastName)
            {
                LastName = lastName;
                UpdatedAt = DateTime.UtcNow;
            }

            if (Phone != phone)
            {
                Phone = phone;
                UpdatedAt = DateTime.UtcNow;
            }
        }

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}