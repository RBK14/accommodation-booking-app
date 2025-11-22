using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Exceptions;
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
        private readonly List<ScheduleSlot> _scheduleSlots = new();

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
        public IReadOnlyCollection<ScheduleSlot> ScheduleSlots => _scheduleSlots.AsReadOnly();

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
            if (hostProfileId == Guid.Empty)
                throw new DomainValidationException("Listing must have a valid HostProfileId.");
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainValidationException("Listing title cannot be empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainValidationException("Listing description cannot be empty.");
            if (address is null)
                throw new DomainValidationException("Listing must have an address.");
            if (pricePerDay is null)
                throw new DomainValidationException("Listing must have a price per day.");
            if (beds <= 0)
                throw new DomainValidationException("Listing must have at least one bed.");
            if (maxGuests <= 0)
                throw new DomainValidationException("Listing must allow at least one guest.");

            HostProfileId = hostProfileId;
            Title = title.Trim();
            Description = description.Trim();
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
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainValidationException("Listing title cannot be empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainValidationException("Listing description cannot be empty.");
            if (address is null)
                throw new DomainValidationException("Listing must have an address.");
            if (pricePerDay is null)
                throw new DomainValidationException("Listing must have a price per day.");
            if (beds <= 0)
                throw new DomainValidationException("Listing must have at least one bed.");
            if (maxGuests <= 0)
                throw new DomainValidationException("Listing must allow at least one guest.");

            bool modified = false;

            if (Title != title.Trim()) { Title = title.Trim(); modified = true; }
            if (Description != description.Trim()) { Description = description.Trim(); modified = true; }
            if (AccommodationType != accommodationType) { AccommodationType = accommodationType; modified = true; }
            if (Beds != beds) { Beds = beds; modified = true; }
            if (MaxGuests != maxGuests) { MaxGuests = maxGuests; modified = true; }
            if (!Address.Equals(address)) { Address = address; modified = true; }
            if (!PricePerDay.Equals(pricePerDay)) { PricePerDay = pricePerDay; modified = true; }

            if (modified)
                UpdatedAt = DateTime.UtcNow;
        }

        public void ReserveDates(Guid reservationId, DateTime start, DateTime end)
        {
            if (reservationId == Guid.Empty)
                throw new DomainValidationException("Reservation ID cannot be empty.");
            if (start >= end)
                throw new DomainValidationException("Start date must be before end date.");
            if (IsOverlapping(start, end))
                throw new DomainValidationException("The specified time range is already reserved.");
            if (_reservationIds.Contains(reservationId))
                throw new DomainIllegalStateException("This reservation already exists for this listing.");

            var slot = ScheduleSlot.Create(reservationId, start, end);
            _scheduleSlots.Add(slot);
            _reservationIds.Add(reservationId);
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelReservation(Guid reservationId)
        {
            var slot = _scheduleSlots.FirstOrDefault(s => s.ReservationId == reservationId);
            if (slot is null)
                throw new DomainIllegalStateException("Reservation not found for this listing.");

            _scheduleSlots.Remove(slot);
            _reservationIds.Remove(reservationId);
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverlapping(DateTime start, DateTime end)
        {
            return _scheduleSlots.Any(s =>
                start < s.EndDate && end > s.StartDate);
        }

        public void AddReview(Guid guestProfileId, int rating, string comment)
        {
            if (_reviews.Any(r => r.GuestProfileId == guestProfileId))
                throw new DomainIllegalStateException("Guest has already reviewed this listing.");

            var review = Review.Create(guestProfileId, rating, comment);
            _reviews.Add(review);

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateReview(Guid reviewId, Guid guestProfileId, string comment, int? rating = null)
        {
            var review = _reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review is null)
                throw new DomainIllegalStateException("Review not found.");

            review.Update(comment, rating);
            UpdatedAt = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private Listing() { }
#pragma warning restore CS8618
    }
}
