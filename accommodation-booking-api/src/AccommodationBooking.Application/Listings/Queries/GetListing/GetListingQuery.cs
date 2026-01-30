using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetListing
{
    public record GetListingQuery(Guid ListingId) : IRequest<ErrorOr<Listing>>;
}
