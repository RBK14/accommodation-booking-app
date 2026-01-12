using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReviewCommand, ErrorOr<Review>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Review>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetByIdAsync(command.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            // Sprawdzenie, czy gość dodał już opinię
            if (listing.Reviews.Any(r => r.GuestProfileId == command.GuestProfileId))
                return Error.Conflict(
                    "Review.AlreadyExists",
                    "Możesz wystawić tylko jedną opinię dla danej oferty.");

            // Sprawdzamy, czy gość ma zakończoną rezerwację związaną z tą ofertą
            var filters = new List<IFilterable<Reservation>>
            {
                new Reservations.Common.ListingIdFilter(command.ListingId),
                new Reservations.Common.GuestProfileIdFilter(command.GuestProfileId)
            };

            var guestReservationHistory = await _unitOfWork.Reservations.SearchAsync(filters, cancellationToken);

            bool hasCompletedReservation = guestReservationHistory.Any(r => r.Status == ReservationStatus.Completed);

            if (!hasCompletedReservation)
                return Error.Conflict(
                    "Review.NoCompletedReservation",
                    "Nie możesz wystawić opinii, ponieważ nie posiadasz historii rezerwacji związanej z tą ofertą.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            Review review;

            try
            {
                listing.AddReview(
                    command.GuestProfileId,
                    command.Rating,
                    command.Comment);

                review = listing.Reviews.First(r => r.GuestProfileId == command.GuestProfileId);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure(
                    "Review.CreationFailed",
                    "Nie udało się utworzyć opinii.");
            }

            return review;
        }
    }
}
