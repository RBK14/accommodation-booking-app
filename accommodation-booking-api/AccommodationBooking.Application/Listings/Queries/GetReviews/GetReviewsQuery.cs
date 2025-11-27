using AccommodationBooking.Domain.ListingAggregate.Entities;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetReviews
{
    public record GetReviewsQuery(
        Guid? ListingId,
        Guid? GuestProfileId) : IRequest<IEnumerable<Review>>;
}
