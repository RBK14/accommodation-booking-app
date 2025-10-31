using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using AccommodationBooking.Domain.ListingAggregate.Enums;

namespace AccommodationBooking.Domain.ListingAggregate
{
    public class Listing : AggregateRoot<Guid>
    {
        private readonly List<Guid> _reservationIds = new();
        private readonly List<Review> _reviews = new();

        public Guid HostProfileId { get; init; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public AccommodationType AccommodationType { get; private set; }
        public int Beds { get; private set; }
        public int MaxGuests { get; private set; }
        public Address Address { get; private set; }
        public Price PricePerDay { get; private set; }

        public IReadOnlyCollection<Guid> ReservationIds => _reservationIds.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

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
            decimal amountPerDay,
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
                Price.Create(amountPerDay, currency),
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        public void UpdateListing(
            string title,
            string description,
            AccommodationType accommodationType,
            int beds,
            int maxGuests,
            Address address,
            Price pricePerDay)
        {
            if (Title != title)
            {
                Title = title.Trim();
                UpdatedAt = DateTime.UtcNow;
            }

            if (Description != description)
            {
                Description = description.Trim();
                UpdatedAt = DateTime.UtcNow;
            }

            if (AccommodationType != accommodationType)
            {
                AccommodationType = accommodationType;
                UpdatedAt = DateTime.UtcNow;
            }

            if (MaxGuests != maxGuests)
            {
                MaxGuests = maxGuests;
                UpdatedAt = DateTime.UtcNow;
            }

            if (Beds != beds)
            {
                Beds = beds;
                UpdatedAt = DateTime.UtcNow;
            }

            if (Address != address)
            {
                Address = address;
                UpdatedAt = DateTime.UtcNow;
            }

            if (PricePerDay != pricePerDay)
            {
                PricePerDay = pricePerDay;
                UpdatedAt = DateTime.UtcNow;
            }
        }

#pragma warning disable CS8618
        private Listing() { }
#pragma warning restore CS8618
    }
}
