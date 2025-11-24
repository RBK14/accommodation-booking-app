namespace AccommodationBooking.Contracts.Reservations
{
    public record ReservationResponse(
        Guid ListingId,
        Guid GuestProfileId,
        Guid HostProfileId,
        string Title,
        string Country,
        string City,
        string PostalCode,
        string Street,
        string BuildingNumber,
        decimal PricePerDay,
        decimal TotalPrice,
        string Currency,
        string Status,
        DateTime CheckIn,
        DateTime CheckOut);
}
