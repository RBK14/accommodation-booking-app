using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.ReservationAggregate;

namespace AccommodationBooking.Application.Reservations.Common
{
    public class ListingIdFilter(Guid? listingId) : IFilterable<Reservation>
    {
        private readonly Guid? _listingId = listingId;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_listingId is not null && !string.IsNullOrWhiteSpace(_listingId.ToString()))
                query = query.Where(r => r.ListingId.Equals(_listingId));

            return query;
        }
    }

    public class GuestProfileIdFilter(Guid? guestProfileId) : IFilterable<Reservation>
    {
        private readonly Guid? _guestProfileId = guestProfileId;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_guestProfileId is not null && !string.IsNullOrWhiteSpace(_guestProfileId.ToString()))
                query = query.Where(r => r.GuestProfileId.Equals(_guestProfileId));

            return query;
        }
    }

    public class HostProfileIdFilter(Guid? hostProfileId) : IFilterable<Reservation>
    {
        private readonly Guid? _hostProfileId = hostProfileId;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_hostProfileId is not null && !string.IsNullOrWhiteSpace(_hostProfileId.ToString()))
                query = query.Where(r => r.HostProfileId.Equals(_hostProfileId));

            return query;
        }
    }

    public class CheckInRangeFilter(
        DateTime? from,
        DateTime? to) : IFilterable<Reservation>
    {
        private readonly DateTime? _from = from;
        private readonly DateTime? _to = to;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_from.HasValue)
                query = query.Where(r => r.CheckIn >= _from.Value);

            if (_to.HasValue)
                query = query.Where(r => r.CheckIn <= _to.Value);

            return query;
        }
    }

    public class CheckOutRangeFilter(
        DateTime? from,
        DateTime? to) : IFilterable<Reservation>
    {
        private readonly DateTime? _from = from;
        private readonly DateTime? _to = to;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_from.HasValue)
                query = query.Where(r => r.CheckOut >= _from.Value);

            if (_to.HasValue)
                query = query.Where(r => r.CheckOut <= _to.Value);

            return query;
        }
    }

    public class ReservationDateRangeFilter(
        DateTime? from,
        DateTime? to) : IFilterable<Reservation>
    {
        private readonly DateTime? _from = from;
        private readonly DateTime? _to = to;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_from is null && _to is null)
                return query;

            return query.Where(r =>
                (!_from.HasValue || r.CheckOut >= _from.Value) &&
                (!_to.HasValue || r.CheckIn <= _to.Value)
            );
        }
    }
}   
