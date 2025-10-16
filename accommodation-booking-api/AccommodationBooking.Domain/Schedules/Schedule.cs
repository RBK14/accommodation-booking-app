namespace AccommodationBooking.Domain.Schedules
{
    public class Schedule
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
