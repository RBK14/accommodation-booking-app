using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ReservationAggregate;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly List<Reservation> _reservations = new();

        public void Add(Reservation reservation)
        {
            _reservations.Add(reservation);
        }

        public Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(reservation);
        }
        public Task<IEnumerable<Reservation>> SearchAsync(IEnumerable<IFilterable<Reservation>> filters, CancellationToken cancellationToken = default)
        {
            var reservationsQuery = _reservations.AsQueryable();

            if (filters is not null)
            {
                foreach (var filter in filters)
                {
                    reservationsQuery = filter.Apply(reservationsQuery);
                }
            }

            return Task.FromResult <IEnumerable<Reservation>>(reservationsQuery.ToList());
        }

        public void Update(Reservation reservation)
        {
            return;
        }
        public void Remove(Reservation reservation)
        {
            return;
        }
    }
}
