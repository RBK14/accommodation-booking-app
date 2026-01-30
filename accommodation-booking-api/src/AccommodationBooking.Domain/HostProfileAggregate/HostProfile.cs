using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.HostProfileAggregate
{
    /// <summary>
    /// Represents a host profile aggregate root.
    /// </summary>
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
            if (userId == Guid.Empty)
                throw new DomainValidationException("HostProfile must be associated with a valid UserId.");

            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Creates a new host profile for a user.
        /// </summary>
        public static HostProfile Create(Guid userId)
        {
            return new HostProfile(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        /// <summary>
        /// Associates a listing with this host profile.
        /// </summary>
        public void AddListingId(Guid listingId)
        {
            if (listingId == Guid.Empty)
                throw new DomainValidationException("Listing ID cannot be empty.");

            if (_listingIds.Contains(listingId))
                throw new DomainIllegalStateException("The listing is already associated with this host.");

            _listingIds.Add(listingId);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes a listing association from this host profile.
        /// </summary>
        public void RemoveListingId(Guid listingId)
        {
            if (_listingIds.Contains(listingId))
            {
                _listingIds.Remove(listingId);
                UpdatedAt = DateTime.UtcNow;
            }
        }

#pragma warning disable CS8618
        private HostProfile() { }
#pragma warning restore CS8618
    }
}
