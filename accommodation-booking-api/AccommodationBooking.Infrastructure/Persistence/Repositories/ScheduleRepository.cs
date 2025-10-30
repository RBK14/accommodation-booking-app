using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ScheduleAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly List<Schedule> _schedules = new();
        public void Add(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        public Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(schedule);
        }

        public void Update(Schedule schedule)
        {
            return;
        }
        public void Remove(Schedule schedule)
        {
            return;
        }
    }
}
