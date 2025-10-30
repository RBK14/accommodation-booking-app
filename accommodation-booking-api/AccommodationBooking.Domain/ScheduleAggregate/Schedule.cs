using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ScheduleAggregate
{
    public class Schedule : AggregateRoot<Guid>
    {
        private Schedule(Guid id) : base(id) { }

        public static Schedule Create()
        {
            return new Schedule(Guid.NewGuid());
        }

#pragma warning disable CS8618
        private Schedule() { }
#pragma warning restore CS8618
    }
}
