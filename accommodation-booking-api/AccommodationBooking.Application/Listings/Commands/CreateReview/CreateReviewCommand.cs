using AccommodationBooking.Domain.ListingAggregate.Entities;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.CreateReview
{
    public record CreateReviewCommand(
        Guid ListingId,
        Guid GuestProfileId,
        int Rating,
        string Comment) : IRequest<ErrorOr<Review>>;
}
