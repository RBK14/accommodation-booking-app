using AccommodationBooking.Domain.ListingAggregate.Entities;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.UpdateReview
{
    public record UpdateReviewCommand(
        Guid ReviewId,
        Guid ProfileId,
        int Rating,
        string Comment) : IRequest<ErrorOr<Review>>;
}
