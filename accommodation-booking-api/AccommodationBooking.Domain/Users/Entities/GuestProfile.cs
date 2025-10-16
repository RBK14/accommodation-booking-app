using AccommodationBooking.Domain.Reservations;

namespace AccommodationBooking.Domain.Users.Entities
{
    public class GuestProfile
    {
        private readonly List<Reservation> _reservations = new();

        public Guid Id { get; init; }
        public Guid UserId { get; init; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }

        public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private GuestProfile(
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

        public static GuestProfile Create(
            Guid userId,
            string firstName,
            string lastName,
            string phone)
        {
            return new GuestProfile(
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
