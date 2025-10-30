using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ScheduleAggregate
{
    public class Schedule : AggregateRoot<Guid>
    {
        public Guid Id { get; init; }

        private Schedule(Guid id)
        {
            Id = id;
        }

        public static Schedule Create(Guid id)
        {
            return new Schedule(id);
        }
    }
}
