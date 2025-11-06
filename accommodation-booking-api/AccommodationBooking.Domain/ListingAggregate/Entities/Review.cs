using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.ListingAggregate.Entities
{
    public class Review : Entity<Guid>
    {
        public Guid GuestProfileId { get; init; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; private set; }

        private Review(
            Guid id,
            Guid guestProfileId,
            int rating,
            string comment,
            DateTime createdAt) : base(id)
        {
            GuestProfileId = guestProfileId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }

        internal static Review Create(Guid guestProfileId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            return new Review(
                Guid.NewGuid(),
                guestProfileId,
                rating,
                comment.Trim(),
                DateTime.UtcNow);
        }

        internal void Update(string comment, int? newRating = null)
        {
            Comment = comment.Trim();

            if (newRating.HasValue)
            {
                if (newRating.Value < 1 || newRating.Value > 5)
                    throw new Exception("Rating must be between 1 and 5.");

                Rating = newRating.Value;
            }

            UpdatedAt = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private Review() { }
#pragma warning restore CS8618
    }
}
