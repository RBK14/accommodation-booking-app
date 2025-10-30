using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    public class Review : Entity<Guid>
    {
        private Review(Guid id) : base(id) { }

        public static Review Create()
        {
            return new Review(Guid.NewGuid());
        }
#pragma warning disable CS8618
        private Review() { }
#pragma warning restore CS8618
    }
}
