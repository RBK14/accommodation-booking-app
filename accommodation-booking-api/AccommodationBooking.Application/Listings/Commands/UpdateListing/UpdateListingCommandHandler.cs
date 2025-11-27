using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.Common.Exceptions;

namespace AccommodationBooking.Application.Listings.Commands.UpdateListing
{
    public class UpdateListingCommandHandler(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Listing>> Handle(UpdateListingCommand command, CancellationToken cancellationToken)
        {

            if (await _unitOfWork.Listings.GetByIdAsync(command.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            var isAdmin = command.ProfileId == Guid.Empty;
            var isOwner = command.ProfileId == listing.HostProfileId;

            if (!isAdmin && !isOwner)
                return Error.Forbidden(
                    "Reservation.InvalidOwner",
                    "Nie posiadasz uprawnień do edycji tej oferty.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var accommodationType = AccommodationTypeExtensions.Parse(command.AccommodationType);
                var currency = CurrencyExtensions.Parse(command.Currency);

                var address = Address.Create(
                    command.Country,
                    command.City,
                    command.PostalCode,
                    command.Street,
                    command.BuildingNumber);

                var pricePerDay = Price.Create(command.AmountPerDay, currency);

                listing.UpdateListing(
                    command.Title,
                    command.Description,
                    accommodationType,
                    command.Beds,
                    command.MaxGuests,
                    address,
                    pricePerDay);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Listing.UpdateFailed;
            }

            return listing;
        }
    }
}
