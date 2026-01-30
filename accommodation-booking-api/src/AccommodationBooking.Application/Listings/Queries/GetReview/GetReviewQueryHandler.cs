using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetReview
{
    public class GetReviewQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReviewQuery, ErrorOr<Review>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Review>> Handle(GetReviewQuery query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetReviewByIdAsync(query.ReviewId, cancellationToken) is not Review review)
                return Errors.Review.NotFound;

            return review;
        }
    }
}
