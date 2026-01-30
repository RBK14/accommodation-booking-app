using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Application.Listings.Common;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReviewCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
        {
            var listings = await _unitOfWork.Listings.SearchAsync(
                new List<IFilterable<Listing>> { new ReviewIdFilter(command.ReviewId) },
                cancellationToken);

            var listing = listings.FirstOrDefault();
            if (listing is null)
                return Errors.Listing.NotFound;

            var review = listing.Reviews.FirstOrDefault(r => r.Id == command.ReviewId);
            if (review is null)
                return Errors.Review.NotFound;

            bool isAdmin = command.ProfileId == Guid.Empty;
            bool isOwner = command.ProfileId == review.GuestProfileId;

            if (!isAdmin && !isOwner)
                return Error.Forbidden(
                       "Review.InvalidOwnerDelete",
                       "Nie posiadasz uprawnien do edycji tej opinii.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                listing.DeleteReview(review.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {

                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure(
                    "Review.DeleteFailed",
                    "Nie udalo sie usunac oferty.");
            }

            return Unit.Value;
        }
    }
}
