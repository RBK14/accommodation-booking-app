namespace AccommodationBooking.Contracts.Authentication
{
    public record UpdatePasswordRequest(
        string Password,
        string NewPassword);
}
