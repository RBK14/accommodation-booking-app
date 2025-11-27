using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Listings.Common;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetReviews
{
    public class GetReviewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReviewsQuery, IEnumerable<Review>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Review>> Handle(GetReviewsQuery query, CancellationToken cancellationToken)
        {
            var listingId = query.ListingId ?? null;
            var guestProfileId = query.GuestProfileId ?? null;

            var filters = new List<IFilterable<Review>>
            {
                new ListingIdFilter(listingId),
                new GuestProfileIdFilter(guestProfileId)
            };

            return await _unitOfWork.Listings.SearchReviewsAsync(filters, cancellationToken);
        }
    }
}
