using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.ReservationAggregate;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository(AppDbContext context) : IReservationRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
        }

        public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Reservation>> SearchAsync(IEnumerable<IFilterable<Reservation>> filters, CancellationToken cancellationToken = default)
        {
            var query = _context.Reservations.AsQueryable();

            if (filters is not null)
            {
                foreach (var filter in filters)
                    query = filter.Apply(query);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public void Remove(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
        }

        public void RemoveRange(IEnumerable<Reservation> reservations)
        {
            if (reservations.Any())
                _context.Reservations.RemoveRange(reservations);
        }
    }
}
