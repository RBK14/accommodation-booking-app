using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.ReservationAggregate.Enums;

namespace AccommodationBooking.Domain.ReservationAggregate
{
    public class Reservation : AggregateRoot<Guid>
    {
        public Guid ListingId { get; init; }
        public Guid GuestProfileId { get; init; }
        public Guid HostProfileId { get; init; }

        public string ListingTitle { get; private set; }
        public Address ListingAddress { get; private set; }
        public Price ListingPricePerDay { get; private set; }

        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public Price TotalPrice { get; private set; }

        public ReservationStatus Status { get; private set; }

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private Reservation(
            Guid id,
            Guid listingId,
            Guid guestProfileId,
            Guid hostProfileId,
            string listingTitle,
            Address listingAddress,
            Price listingPricePerDay,
            DateTime checkIn,
            DateTime checkOut,
            Price totalPrice,
            ReservationStatus status,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            ListingId = listingId;
            GuestProfileId = guestProfileId;
            HostProfileId = hostProfileId;
            ListingTitle = listingTitle;
            ListingAddress = listingAddress;
            ListingPricePerDay = listingPricePerDay;
            CheckIn = checkIn;
            CheckOut = checkOut;
            TotalPrice = totalPrice;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Reservation Create(
            Guid listingId,
            Guid guestProfileId,
            Guid hostProfileId,
            string listingTitle,
            Address listingAddress,
            Price listingPricePerDay,
            DateTime checkIn,
            DateTime checkOut)
        {
            return new Reservation(
                Guid.NewGuid(),
                listingId,
                guestProfileId,
                hostProfileId,
                listingTitle,
                listingAddress,
                listingPricePerDay,
                checkIn,
                checkOut,
                CalculateTotalPrice(checkIn, checkOut, listingPricePerDay),
                ReservationStatus.Accepted,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        private static Price CalculateTotalPrice(
            DateTime checkIn,
            DateTime checkOut,
            Price pricePerDay)
        {
            var days = (checkOut - checkIn).Days;
            if (days <= 0)
                throw new Exception("Check-out date must be later than check-in date.");

            var totalAmount = pricePerDay.Amount * days;
            return Price.Create(totalAmount, pricePerDay.Currency);
        }

        public void MarkAsInProgress()
        {
            if (Status != ReservationStatus.Accepted)
                throw new Exception("Reservation must be accepted to mark as in progress.");

            Status = ReservationStatus.InProgress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            if (Status != ReservationStatus.InProgress)
                throw new Exception("Reservation must be in progress to complete.");

            Status = ReservationStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == ReservationStatus.Completed)
                throw new Exception("Cannot cancel a completed reservation.");

            Status = ReservationStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsNoShow()
        {
            if (Status != ReservationStatus.InProgress)
                throw new Exception("Reservation must be in progress to mark as no-show.");

            Status = ReservationStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private Reservation() { }
#pragma warning restore CS8618
    }
}
