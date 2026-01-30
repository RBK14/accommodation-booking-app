using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetAvailableDates
{
    public record GetAvailableDatesQuery(
        Guid ListingId,
        DateOnly? From = null,
        int? Days = null) : IRequest<ErrorOr<IEnumerable<DateOnly>>>;
}
