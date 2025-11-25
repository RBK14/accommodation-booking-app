using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetAvailableDates
{
    public record GetAvailableDatesQuery(
        Guid ListingId,
        DateOnly? From = null,
        int Days = 14) : IRequest<ErrorOr<IEnumerable<DateOnly>>>;
}
