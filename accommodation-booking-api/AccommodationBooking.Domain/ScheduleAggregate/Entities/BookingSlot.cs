using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ScheduleAggregate.Entities
{
    public class BookingSlot : Entity<Guid>
    {
        private BookingSlot(Guid id) : base(id) { }

        public static BookingSlot Create()
        {
            return new BookingSlot(Guid.NewGuid());
        }

#pragma warning disable CS8618
        private BookingSlot() { }
#pragma warning restore CS8618
    }
}
