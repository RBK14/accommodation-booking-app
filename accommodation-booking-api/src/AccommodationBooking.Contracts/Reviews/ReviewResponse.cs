namespace AccommodationBooking.Contracts.Reviews
{
    public record ReviewResponse(
        Guid ListingId,
        Guid GuestProfileId,
        int Rating,
        string Comment);
}
