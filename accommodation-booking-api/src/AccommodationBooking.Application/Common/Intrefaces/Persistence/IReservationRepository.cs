using AccommodationBooking.Domain.ReservationAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    /// <summary>
    /// Repository interface for reservation entities.
    /// </summary>
    public interface IReservationRepository
    {
        void Add(Reservation reservation);
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Reservation>> SearchAsync(IEnumerable<IFilterable<Reservation>> filters, CancellationToken cancellationToken = default);
        void Remove(Reservation reservation);
        void RemoveRange(IEnumerable<Reservation> reservations);
    }
}
