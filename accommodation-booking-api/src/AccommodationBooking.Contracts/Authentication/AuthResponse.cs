namespace AccommodationBooking.Contracts.Authentication
{
    public record AuthResponse(
        Guid Id,
        string AccessToken);
}
