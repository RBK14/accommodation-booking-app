using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.HostProfileAggregate;
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
            if (await _unitOfWork.HostProfiles.GetByIdAsync(command.HostProfileId, cancellationToken) is not HostProfile hostProfile)
                return Errors.HostProfile.NotFound;

            var accommodationType = AccommodationTypeExtensions.Parse(command.AccommodationType);
            var currency = CurrencyExtensions.Parse(command.Currency);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            Listing listing;

            try
            {
                listing = Listing.Create(
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

                listing.UpdatePhotos(command.Photos);

                hostProfile.AddListingId(listing.Id);
                _unitOfWork.Listings.Add(listing);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Listing.CreationFailed;
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
