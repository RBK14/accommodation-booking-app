using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.GuestProfileAggregate
{
    public class GuestProfile : AggregateRoot<Guid>
    {
        private readonly List<Guid> _reservationIds = new();

        public Guid UserId { get; init; }

        public IReadOnlyCollection<Guid> ReservationIds => _reservationIds.AsReadOnly();

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private GuestProfile(
            Guid id,
            Guid userId,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static GuestProfile Create(Guid userId)
        {
            return new GuestProfile(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

#pragma warning disable CS8618
        private GuestProfile() { }
#pragma warning restore CS8618
    }
}
