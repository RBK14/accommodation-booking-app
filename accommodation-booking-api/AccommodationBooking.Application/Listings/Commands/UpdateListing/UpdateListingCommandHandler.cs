using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using AccommodationBooking.Domain.Common.ValueObjects;

namespace AccommodationBooking.Application.Listings.Commands.UpdateListing
{
    public class UpdateListingCommandHandler(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Listing>> Handle(UpdateListingCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetByIdAsync(command.ListingId) is not Listing listing)
                return Errors.Listing.NotFound;

            try
            {
                var accommodationType = AccommodationTypeExtensions.ParseAccommodationType(command.AccommodationType);
                var currency = CurrencyExtensions.ParseCurrency(command.Currency);

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
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Listing.UpdateFailed;
            }

            return listing;
        }
    }
}
