using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.UpdateListing
{
    public record UpdateListingCommand(
        Guid ListingId,
        Guid ProfileId,
        string Title,
        string Description,
        string AccommodationType,
        int Beds,
        int MaxGuests,
        string Country,
        string City,
        string PostalCode,
        string Street,
        string BuildingNumber,
        decimal AmountPerDay,
        string Currency,
        IEnumerable<string> Photos) : IRequest<ErrorOr<Listing>>;
}
