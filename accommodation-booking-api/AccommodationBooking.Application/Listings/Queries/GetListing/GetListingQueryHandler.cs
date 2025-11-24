using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetListing
{
    public class GetListingQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListingQuery, ErrorOr<Listing>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Listing>> Handle(GetListingQuery query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetByIdAsync(query.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            return listing;
        }
    }
}
