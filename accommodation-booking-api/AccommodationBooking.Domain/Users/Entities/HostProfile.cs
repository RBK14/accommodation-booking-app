using AccommodationBooking.Domain.Listings;

namespace AccommodationBooking.Domain.Users.Entities
{
    public class HostProfile
    {
        private readonly List<Listing> _listings = new();

        public Guid Id { get; init; }
        public Guid UserId { get; init; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }

        public IReadOnlyCollection<Listing> Listings => _listings.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private HostProfile(
            Guid id,
            Guid userId,
            string firstName,
            string lastName,
            string phone,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static HostProfile Create(
            Guid userId,
            string firstName,
            string lastName,
            string phone)
        {
            return new HostProfile(
                Guid.NewGuid(),
                userId,
                firstName,
                lastName,
                phone,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateFirstName(string firstName)
        {
            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLastName(string lastName)
        {
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePhone(string phone)
        {
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
