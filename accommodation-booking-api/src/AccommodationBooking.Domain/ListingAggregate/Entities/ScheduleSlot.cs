using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    public class ScheduleSlot : Entity<Guid>
    {
        public Guid ReservationId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private ScheduleSlot(
            Guid id,
            Guid reservationId,
            DateTime startDate,
            DateTime endDate,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id)
        {
            if (reservationId == Guid.Empty)
                throw new DomainValidationException("Reservation ID cannot be empty.");
            if (startDate < DateTime.UtcNow)
                throw new DomainValidationException("Start date cannot be in the past.");
            if (endDate <= startDate)
                throw new DomainValidationException("End date must be after start date.");

            ReservationId = reservationId;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        internal static ScheduleSlot Create(
            Guid reservationId,
            DateTime start,
            DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End date must be after start date.");

            return new ScheduleSlot(
                Guid.NewGuid(),
                reservationId,
                start,
                end,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

#pragma warning disable CS8618
        private ScheduleSlot() { }
#pragma warning restore CS8618
    }
}
