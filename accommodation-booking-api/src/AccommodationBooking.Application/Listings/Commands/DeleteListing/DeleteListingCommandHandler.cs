using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Application.Reservations.Common;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.DeleteListing
{
    public class DeleteListingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteListingCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteListingCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetByIdAsync(command.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            if (listing.HostProfileId != command.HostProfileId && command.HostProfileId != Guid.Empty)
                return Error.Forbidden(
                    "Listing.InvalidOwner",
                    "Nie posiadasz uprawnien do usuniecia tej oferty.");

            var listingReservations = await _unitOfWork.Reservations.SearchAsync(new List<IFilterable<Reservation>> { new ListingIdFilter(listing.Id) }, cancellationToken);

            bool hasActiveReservations = listingReservations.Any(r =>
                r.Status == ReservationStatus.Accepted ||
                r.Status == ReservationStatus.InProgress);

            if (hasActiveReservations)
            {
                return Error.Conflict(
                    "Listing.CannotDeleteActive",
                    "Nie mozna usunac oferty, która posiada nadchodzace lub trwajace rezerwacje");
            }

            if (await _unitOfWork.HostProfiles.GetByIdAsync(listing.HostProfileId, cancellationToken) is not HostProfile hostProfile)
                return Errors.HostProfile.NotFound;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                hostProfile.RemoveListingId(listing.Id);

                _unitOfWork.Reservations.RemoveRange(listingReservations);
                _unitOfWork.Listings.Remove(listing);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure(
                    "Listing.DeleteFailed",
                    "Nie udalo sie usunac oferty.");
            }

            return Unit.Value;
        }
    }
}
