using AccommodationBooking.Domain.ReservationAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IReservationRepository
    {
        void Add(Reservation reservation);
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Update(Reservation reservation);
        void Remove(Reservation reservation);
    }
}
