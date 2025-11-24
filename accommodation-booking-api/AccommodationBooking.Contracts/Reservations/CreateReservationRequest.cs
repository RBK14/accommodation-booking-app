namespace AccommodationBooking.Contracts.Reservations
{
    public record CreateReservationRequest(
        Guid ListingId,
        DateOnly CheckIn,
        DateOnly CheckOut);
}
