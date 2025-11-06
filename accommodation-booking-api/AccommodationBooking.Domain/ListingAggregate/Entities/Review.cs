using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    public class Review : Entity<Guid>
    {
        public Guid GuestProfileId { get; init; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }

        private Review(
            Guid id,
            Guid guestProfileId,
            int rating,
            string comment,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            if (guestProfileId == Guid.Empty)
                throw new DomainValidationException("GuestProfileId cannot be empty.");

            if (rating < 1 || rating > 5)
                throw new DomainValidationException("Rating must be between 1 and 5.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new DomainValidationException("Comment cannot be empty.");

            GuestProfileId = guestProfileId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        internal static Review Create(Guid guestProfileId, int rating, string comment)
        {
            return new Review(
                Guid.NewGuid(),
                guestProfileId,
                rating,
                comment.Trim(),
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

        internal void Update(string comment, int? newRating = null)
        {
            if (string.IsNullOrWhiteSpace(comment))
                throw new DomainValidationException("Comment cannot be empty.");

            Comment = comment.Trim();

            if (newRating.HasValue)
            {
                if (newRating.Value < 1 || newRating.Value > 5)
                    throw new DomainValidationException("Rating must be between 1 and 5.");

                Rating = newRating.Value;
            }

            UpdatedAt = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private Review() { }
#pragma warning restore CS8618
    }
}
