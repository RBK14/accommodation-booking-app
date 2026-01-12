using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Reservations.Common;
using AccommodationBooking.Domain.ReservationAggregate;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Queries.GetReservations
{
    public class GetReservationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReservationsQuery, IEnumerable<Reservation>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Reservation>> Handle(GetReservationsQuery query, CancellationToken cancellationToken)
        {
            var listingId = query.ListingId ?? null;
            var guestProfileId = query.GuestProfileId ?? null;
            var hostProfileId = query.HostProfileId ?? null;

            var filters = new List<IFilterable<Reservation>>
            {
                new ListingIdFilter(listingId),
                new GuestProfileIdFilter(guestProfileId),
                new HostProfileIdFilter(hostProfileId)
            };

            return await _unitOfWork.Reservations.SearchAsync(filters, cancellationToken);
        }
    }
}
