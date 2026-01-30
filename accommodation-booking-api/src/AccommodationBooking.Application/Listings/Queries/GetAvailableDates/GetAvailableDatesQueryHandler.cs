using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Queries.GetAvailableDates
{
    public class GetAvailableDatesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvailableDatesQuery, ErrorOr<IEnumerable<DateOnly>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<IEnumerable<DateOnly>>> Handle(GetAvailableDatesQuery query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Listings.GetByIdAsync(query.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            var searchStart = query.From ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var searchEnd = searchStart.AddDays(query.Days ?? 14);

            var occupiedDates = new HashSet<DateOnly>();

            var relevantSlots = listing.ScheduleSlots
                .Where(slot => DateOnly.FromDateTime(slot.EndDate) > searchStart && DateOnly.FromDateTime(slot.StartDate) <= searchEnd);

            foreach (var slot in relevantSlots)
            {
                var start = DateOnly.FromDateTime(slot.StartDate);
                var end = DateOnly.FromDateTime(slot.EndDate);

                for (var date = start; date < end; date = date.AddDays(1))
                {
                    occupiedDates.Add(date);
                }
            }

            var availableSlots = new List<DateOnly>();

            for (var date = searchStart; date <= searchEnd; date = date.AddDays(1))
            {
                if (!occupiedDates.Contains(date))
                {
                    availableSlots.Add(date);
                }
            }

            return availableSlots;
        }
    }
}
