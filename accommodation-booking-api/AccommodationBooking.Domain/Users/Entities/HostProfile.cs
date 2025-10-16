using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Listings;
using System.Text.RegularExpressions;

namespace AccommodationBooking.Domain.Users.Entities
{
    public class HostProfile : Entity<Guid>
    {
        private readonly List<Listing> _listings = new();

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
            DateTime updatedAt) : base(id)
        {
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
            ValidateName(firstName, nameof(firstName));

            if (FirstName == firstName)
                return;

            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLastName(string lastName)
        {
            ValidateName(lastName, nameof(lastName));

            if (LastName == lastName)
                return;

            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePhone(string phone)
        {
            ValidatePhone(phone);

            if (Phone == phone)
                return;

            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateName(string name, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"{propertyName} cannot be empty.");

            if (name.Length > 100)
                throw new Exception($"{propertyName} cannot exceed 100 characters.");
        }

        private static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number cannot be empty.");

            // Walidacja numeru telefonu np. +48 123 456 789 lub 123456789
            var phonePattern = @"^\+?\d{7,15}$";
            if (!Regex.IsMatch(phone, phonePattern))
                throw new ArgumentException("Invalid phone number format.");
        }

#pragma warning disable CS8618
        private HostProfile()
        {
        }
#pragma warning restore CS8618
    }
}
