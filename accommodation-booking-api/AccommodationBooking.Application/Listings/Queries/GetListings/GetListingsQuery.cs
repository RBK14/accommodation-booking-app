using AccommodationBooking.Domain.ListingAggregate;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetListings
{
    public record GetListingsQuery(Guid? HostProfileId) : IRequest<IEnumerable<Listing>>;
}
