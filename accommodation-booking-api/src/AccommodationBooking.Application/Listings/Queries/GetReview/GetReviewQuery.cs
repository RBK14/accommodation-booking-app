using AccommodationBooking.Domain.ListingAggregate.Entities;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetReview
{
    public record GetReviewQuery(Guid ReviewId) : IRequest<ErrorOr<Review>>;
}
