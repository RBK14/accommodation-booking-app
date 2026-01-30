using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.GuestProfileAggregate
{
    /// <summary>
    /// Represents a guest profile aggregate root.
    /// </summary>
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
            if (userId == Guid.Empty)
                throw new DomainValidationException("GuestProfile must be associated with a valid UserId.");

            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Creates a new guest profile for a user.
        /// </summary>
        public static GuestProfile Create(Guid userId)
        {
            return new GuestProfile(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        /// <summary>
        /// Associates a reservation with this guest profile.
        /// </summary>
        public void AddReservationId(Guid reservationId)
        {
            if (reservationId == Guid.Empty)
                throw new DomainValidationException("Reservation ID cannot be empty.");

            if (_reservationIds.Contains(reservationId))
                throw new DomainIllegalStateException("The reservation is already associated with this guest.");

            _reservationIds.Add(reservationId);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes a reservation association from this guest profile.
        /// </summary>
        public void RemoveReservationId(Guid reservationId)
        {
            if (_reservationIds.Contains(reservationId))
            {
                _reservationIds.Remove(reservationId);
                UpdatedAt = DateTime.UtcNow;
            }
        }

#pragma warning disable CS8618
        private GuestProfile() { }
#pragma warning restore CS8618
    }
}
