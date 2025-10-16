using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.Listings.Enums;
using AccommodationBooking.Domain.Reservations;
using AccommodationBooking.Domain.Schedules;

namespace AccommodationBooking.Domain.Listings
{
    public class Listing : Entity<Guid>
    {
        private readonly List<Reservation> _reservations = new();

        public Guid HostProfileId { get; init; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public AccommodationType AccommodationType { get; private set; }
        public int Beds { get; private set; }
        public int MaxGuests { get; private set; }
        public Address Address { get; private set; }
        public Price PricePerDay { get; private set; }

        public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        // Nawigacje
        public Schedule Schedule { get; init; }

        private Listing(
            Guid id,
            Guid hostProfileId,
            string title,
            string description,
            AccommodationType accommodationType,
            int beds,
            int maxGuests,
            Address address,
            Price pricePerDay,
            Schedule schedule,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            HostProfileId = hostProfileId;
            Title = title;
            Description = description;
            AccommodationType = accommodationType;
            Beds = beds;
            MaxGuests = maxGuests;
            Address = address;
            PricePerDay = pricePerDay;
            Schedule = schedule;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Listing Create(
            Guid hostProfileId,
            string title,
            string description,
            AccommodationType accommodationType,
            int beds,
            int maxGuests,
            string country,
            string city,
            string postalCode,
            string street,
            string buildingNumber,
            decimal amount,
            Currency currency)
        {
            var id = Guid.NewGuid();

            return new Listing(
                id,
                hostProfileId,
                title,
                description,
                accommodationType,
                beds,
                maxGuests,
                Address.Create(country, city, postalCode, street, buildingNumber),
                Price.Create(amount, currency),
                Schedule.Create(id),
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title cannot be empty.");

            Title = title.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new Exception("Description cannot be empty.");

            Description = description.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeAccommodationType(AccommodationType type)
        {
            AccommodationType = type;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCapacity(int beds, int maxGuests)
        {
            if (beds <= 0)
                throw new Exception("Number of beds must be greater than zero.");

            if (maxGuests <= 0)
                throw new Exception("Maximum number of guests must be greater than zero.");

            Beds = beds;
            MaxGuests = maxGuests;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePrice(decimal amount, Currency currency)
        {
            if (amount <= 0)
                throw new Exception("Price must be greater than zero.");

            PricePerDay = Price.Create(amount, currency);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeAddress(string country, string city, string postalCode, string street, string buildingNumber)
        {
            Address = Address.Create(country, city, postalCode, street, buildingNumber);
            UpdatedAt = DateTime.UtcNow;
        }


#pragma warning disable CS8618
        private Listing()
        {
        }
#pragma warning restore CS8618
    }
}
