using AccommodationBooking.Domain.Common.Exceptions;
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
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainValidationException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainValidationException("Password hash cannot be empty.");

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
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainValidationException("Email cannot be empty.");

            if (Email == email)
                return;

            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainValidationException("Password hash cannot be empty.");

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
            bool modified = false;

            if (FirstName != firstName) { FirstName = firstName; modified = true; }
            if (LastName != lastName) { LastName = lastName; modified = true; }
            if (Phone != phone) { Phone = phone; modified = true; }

            if (modified)
                UpdatedAt = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}