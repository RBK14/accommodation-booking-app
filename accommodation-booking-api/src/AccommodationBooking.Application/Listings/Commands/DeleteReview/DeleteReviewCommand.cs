using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.DeleteReview
{
    public record DeleteReviewCommand(
        Guid ReviewId,
        Guid ProfileId) : IRequest<ErrorOr<Unit>>;
}
