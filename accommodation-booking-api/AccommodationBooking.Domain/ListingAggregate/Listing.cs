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

        public void AddReview(Guid guestProfileId, int rating, string comment)
        {
            if (_reviews.Any(r => r.GuestProfileId == guestProfileId))
                throw new Exception("Guest has already reviewed this listing.");

            var review = Review.Create(guestProfileId, rating, comment);
            _reviews.Add(review);

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateReview(Guid reviewId, Guid guestProfileId, string comment, int? rating = null)
        {
            var review = _reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review is null)
                throw new Exception("Review not found.");

            if (review.GuestProfileId != guestProfileId)
                throw new Exception("Guest cannot edit this review.");

            review.Update(comment, rating);
            UpdatedAt = DateTime.UtcNow;
        }
        public void ReserveDates(Guid reservationId, DateTime start, DateTime end)
        {
            if (IsOverlapping(start, end))
                throw new Exception("The specified time range is already reserved.");

            var slot = ScheduleSlot.Create(reservationId, start, end);
            _scheduleSlots.Add(slot);
            _reservationIds.Add(reservationId);
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelReservation(Guid reservationId)
        {
            var slot = _scheduleSlots.FirstOrDefault(s => s.ReservationId == reservationId);
            if (slot is null)
                throw new Exception("Reservation not found for this listing.");

            _scheduleSlots.Remove(slot);
            _reservationIds.Remove(reservationId);
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverlapping(DateTime start, DateTime end)
        {
            return _scheduleSlots.Any(s =>
                start < s.End && end > s.Start);
        }

#pragma warning disable CS8618
        private Listing() { }
#pragma warning restore CS8618
    }
}
