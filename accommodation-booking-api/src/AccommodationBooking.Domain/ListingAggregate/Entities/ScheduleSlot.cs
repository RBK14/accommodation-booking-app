using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    /// <summary>
    /// Represents a scheduled time slot for a reservation.
    /// </summary>
    public class ScheduleSlot : Entity<Guid>
    {
        /// <summary>
        /// Gets the reservation identifier associated with this schedule slot.
        /// </summary>
        public Guid ReservationId { get; init; }

        /// <summary>
        /// Gets the start date and time of the schedule slot.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets the end date and time of the schedule slot.
        /// </summary>
        public DateTime EndDate { get; init; }

        /// <summary>
        /// Gets the date and time when the schedule slot was created.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Gets the date and time when the schedule slot was last updated.
        /// </summary>
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

        /// <summary>
        /// Creates a new instance of the <see cref="ScheduleSlot"/> class.
        /// </summary>
        /// <param name="reservationId">The reservation identifier.</param>
        /// <param name="start">The start date and time of the schedule slot.</param>
        /// <param name="end">The end date and time of the schedule slot.</param>
        /// <returns>A new instance of the <see cref="ScheduleSlot"/> class.</returns>
        /// <exception cref="ArgumentException">Thrown when the end date is not after the start date.</exception>
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
