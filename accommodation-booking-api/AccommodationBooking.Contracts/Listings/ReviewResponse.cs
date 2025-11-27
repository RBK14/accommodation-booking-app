namespace AccommodationBooking.Contracts.Listings
{
    public record ReviewResponse(
        Guid ListingId,
        Guid GuestProfileId,
        int Rating,
        string Comment);
}
