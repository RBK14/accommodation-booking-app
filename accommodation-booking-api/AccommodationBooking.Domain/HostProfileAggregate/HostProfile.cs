using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.HostProfileAggregate
{
    public class HostProfile : AggregateRoot<Guid>
    {
        private readonly List<Guid> _listingIds = new();

        public Guid UserId { get; init; }

        public IReadOnlyCollection<Guid> ListingIds => _listingIds.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private HostProfile(
            Guid id,
            Guid userId,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static HostProfile Create(Guid userId)
        {
            return new HostProfile(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

#pragma warning disable CS8618
        private HostProfile() { }
#pragma warning restore CS8618
    }
}
