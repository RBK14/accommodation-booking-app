using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.CreateListing
{
    public class CreateListingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateListingCommand, ErrorOr<Listing>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Listing>> Handle(CreateListingCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.HostProfiles.GetByIdAsync(command.HostProfileId) is null)
                return Errors.HostProfile.NotFound;

            var accommodationType = AccommodationTypeExtensions.Parse(command.AccommodationType);
            var currency = CurrencyExtensions.Parse(command.Currency);

            var listing = Listing.Create(
                command.HostProfileId,
                command.Title,
                command.Description,
                accommodationType,
                command.Beds,
                command.MaxGuests,
                command.Country,
                command.City,
                command.PostalCode,
                command.Street,
                command.BuildingNumber,
                command.AmountPerDay,
                currency);

            try
            {
                _unitOfWork.Listings.Add(listing);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Listing.CreationFailed;
            }

            return listing;
        }
    }
}
