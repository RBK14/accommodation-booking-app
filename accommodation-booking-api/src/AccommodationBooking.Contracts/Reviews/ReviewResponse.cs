namespace AccommodationBooking.Contracts.Reviews
{
    public record ReviewResponse(
        Guid Id,
        Guid ListingId,
        Guid GuestProfileId,
        int Rating,
        string Comment,
        DateTime CreatedAt,
        DateTime UpdatedAt
        );
}