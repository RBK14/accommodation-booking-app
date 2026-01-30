using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateReviewCommand, ErrorOr<Review>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Review>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
        {
            var listings = await _unitOfWork.Listings.SearchAsync(
                new List<IFilterable<Listing>> { new Listings.Common.ReviewIdFilter(command.ReviewId) },
                cancellationToken);

            var listing = listings.FirstOrDefault();
            if (listing is null)
                return Errors.Listing.NotFound;

            var review = listing.Reviews.FirstOrDefault(r => r.Id == command.ReviewId);
            if (review is null)
                return Errors.Review.NotFound;

            // Sprawdzenie uprawnien
            bool isAdmin = command.ProfileId == Guid.Empty;
            bool isOwner = command.ProfileId == review.GuestProfileId;

            if (!isAdmin && !isOwner)
                return Error.Forbidden(
                    "Review.InvalidOwnerUpdate",
                    "Nie posiadasz uprawnien do edycji tej opinii.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                listing.UpdateReview(
                    command.ReviewId,
                    command.Comment,
                    command.Rating);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure(
                   "Review.CreationFailed",
                   "Nie udalo sie utworzyc opinii.");
            }

            return review;
        }
    }
}
