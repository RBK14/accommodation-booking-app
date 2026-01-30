using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Application.Listings.Common;
using AccommodationBooking.Domain.ListingAggregate;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetListings
{
    public class GetListingsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListingsQuery, IEnumerable<Listing>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Listing>> Handle(GetListingsQuery query, CancellationToken cancellationToken)
        {
            var hostProfileId = query.HostProfileId ?? null;

            var filters = new List<IFilterable<Listing>>
            {
                new HostProfileIdFilter(hostProfileId)
            };

            return await _unitOfWork.Listings.SearchAsync(filters, cancellationToken);
        }
    }
}
