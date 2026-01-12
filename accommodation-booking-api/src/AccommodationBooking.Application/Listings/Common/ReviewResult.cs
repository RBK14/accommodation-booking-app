namespace AccommodationBooking.Application.Listings.Common
{
    public record ReviewResult(
        Guid ReviewId,
        Guid ListingId,
        string ListingTitle,
        string GuestFirstName,
        string GuestLastName,
        int Rating,
        string Comment,
        DateTime CreatedAt);
}
