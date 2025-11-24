namespace AccommodationBooking.Contracts.Reservations
{
    public record CreateReservationRequest(
        DateOnly CheckIn,
        DateOnly CheckOut);
}
