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

    public class ListingIdsFilter(IEnumerable<Guid>? listingIds) : IFilterable<Reservation>
    {
        private readonly IEnumerable<Guid>? _listingIds = listingIds;

        public IQueryable<Reservation> Apply(IQueryable<Reservation> query)
        {
            if (_listingIds is not null && _listingIds.Any())
            {
                query = query.Where(r => _listingIds.Contains(r.ListingId));
            }

            return query;
        }
    }
}   
