namespace AccommodationBooking.Contracts.Reviews
{
    public record UpdateReviewRequest(
        int Rating,
        string Comment);
}
