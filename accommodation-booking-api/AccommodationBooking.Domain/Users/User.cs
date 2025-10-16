using AccommodationBooking.Domain.Users.Entities;
using AccommodationBooking.Domain.Users.Enums;

namespace AccommodationBooking.Domain.Users
{
    public class User
    {
        public Guid Id { get; init; }

        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

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
            UserRole role,
            GuestProfile guestProfile,
            HostProfile hostProfile,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
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
                UserRole.Guest,
                GuestProfile.Create(id, firstName, lastName, phone),
                null!,
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
                UserRole.Host,
                null!,
                HostProfile.Create(id, firstName, lastName, phone),
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public static User CreateAdmin(string email, string passwordHash)
        {
            return new User(
                Guid.NewGuid(),
                email,
                passwordHash,
                UserRole.Admin,
                null!,
                null!,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            // TODO: Walidacja hasła

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}