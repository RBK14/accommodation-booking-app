using AccommodationBooking.Domain.ListingAggregate;

namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IListingRepository
    {
        void Add(Listing listing);
        Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Listing>> SearchAsync(IEnumerable<IFilterable<Listing>> filters, CancellationToken cancellationToken = default);
        void Remove(Listing listing);

        // TODO: Ewentaulne wyszukiwanie tylko slotów lub rezerwacji
        // Task<IEnumerable<ScheduleSlot>> GetListingSlotsAsync(Guid id, CancellationToken cancellationToken);
        // Task<IEnumerable<ScheduleSlot>> GetListingReviewAsync(Guid id, CancellationToken cancellationToken);
    }
}
