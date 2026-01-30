using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands.DeleteListing
{
    public record DeleteListingCommand(Guid ListingId, Guid HostProfileId) : IRequest<ErrorOr<Unit>>;
}
