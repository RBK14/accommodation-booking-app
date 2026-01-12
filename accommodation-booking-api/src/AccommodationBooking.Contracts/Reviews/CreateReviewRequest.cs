namespace AccommodationBooking.Contracts.Reviews
{
    public record CreateReviewRequest(
        Guid ListingId,
        int Rating,
        string Comment);
}
