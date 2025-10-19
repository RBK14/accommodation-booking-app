using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Listings;

namespace AccommodationBooking.Domain.Users.Entities
{
    public class HostProfile : Entity<Guid>
    {
        private readonly List<Listing> _listings = new();

        public Guid UserId { get; init; }

        public IReadOnlyCollection<Listing> Listings => _listings.AsReadOnly();

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
        private HostProfile()
        {
        }
#pragma warning restore CS8618
    }
}
