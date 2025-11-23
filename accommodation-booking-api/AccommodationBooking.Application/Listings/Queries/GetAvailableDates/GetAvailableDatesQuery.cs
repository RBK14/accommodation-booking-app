using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetAvailableDates
{
    public record GetAvailableDatesQuery(
        Guid ListingId,
        DateTime? From = null,
        int Days = 14) : IRequest<ErrorOr<IEnumerable<DateOnly>>>;
}
