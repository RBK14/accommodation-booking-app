namespace AccommodationBooking.Contracts.Listings
{
    public record ListingResponse(
        Guid Id,
        Guid HostProfileId,
        string Title,
        string Description,
        string AccommodationType,
        int Beds,
        int MaxGuests,
        string Country,
        string City,
        string PostalCode,
        string Street,
        string BuildingNumber,
        decimal AmountPerDay,
        string Currency);
}
