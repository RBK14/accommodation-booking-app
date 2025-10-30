using AccommodationBooking.Domain.ScheduleAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IScheduleRepository
    {
        void Add(Schedule schedule);
        Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Update(Schedule schedule);
        void Remove(Schedule schedule);
    }
}
