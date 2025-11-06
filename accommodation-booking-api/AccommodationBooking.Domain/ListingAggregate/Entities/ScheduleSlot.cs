using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    public class ScheduleSlot : Entity<Guid>
    {
        public Guid ReservationId { get; init; }
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        private ScheduleSlot(
            Guid id,
            Guid reservationId,
            DateTime start,
            DateTime end)
            : base(id)
        {
            ReservationId = reservationId;
            Start = start;
            End = end;
        }

        internal static ScheduleSlot Create(
            Guid reservationId,
            DateTime start,
            DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time.");

            return new ScheduleSlot(
                Guid.NewGuid(),
                reservationId,
                start,
                end);
        }

#pragma warning disable CS8618
        private ScheduleSlot() { }
#pragma warning restore CS8618
    }
}
