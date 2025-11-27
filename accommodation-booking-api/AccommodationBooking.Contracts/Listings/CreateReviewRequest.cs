namespace AccommodationBooking.Contracts.Listings
{
    public record CreateReviewRequest(
        Guid ListingId,
        int Rating,
        string Comment);
}
