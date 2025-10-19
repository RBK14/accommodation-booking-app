using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Users.Entities;
using AccommodationBooking.Domain.Users.Enums;
using System.Text.RegularExpressions;

namespace AccommodationBooking.Domain.Users
{
    public class User : Entity<Guid>
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone {  get; private set; }

        public UserRole Role { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        // Nawigacje
        public GuestProfile? GuestProfile { get; init; }
        public HostProfile? HostProfile { get; init; }

        private User(
            Guid id,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            string phone,
            UserRole role,
            GuestProfile? guestProfile,
            HostProfile? hostProfile,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Role = role;
            GuestProfile = guestProfile;
            HostProfile = hostProfile;
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
                GuestProfile.Create(id),
                null,
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
                null,
                HostProfile.Create(id),
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
                null,
                null,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateEmail(string email)
        {
            ValidateEmail(email);

            if (Email == email)
                return;

            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            ValidatePasswordHash(passwordHash);

            if (PasswordHash == passwordHash)
                return;

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email cannot be empty.");

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
                throw new Exception("Invalid email format.");
        }

        private static void ValidatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new Exception("Password hash cannot be empty.");
        }

        public void UpdatePersonalDetails(
            string firstName,
            string lastName,
            string phone)
        {
            ValidateName(firstName, nameof(firstName));
            ValidateName(lastName, nameof(lastName));
            ValidatePhone(phone);

            if (FirstName == firstName)
                return;
            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;

            if (LastName == lastName)
                return;
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;

            if (Phone == phone)
                return;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateName(string name, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{propertyName} cannot be empty.");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException($"{propertyName} cannot exceed 100 characters.");
            }
        }

        private static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Phone number cannot be empty.");
            }

            var phonePattern = @"^\+?[1-9]\d{1,14}$";
            if (!Regex.IsMatch(phone, phonePattern))
            {
                throw new ArgumentException("Invalid phone number format.");
            }
        }

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}