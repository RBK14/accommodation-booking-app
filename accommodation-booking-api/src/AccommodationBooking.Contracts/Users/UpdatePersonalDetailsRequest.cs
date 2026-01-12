namespace AccommodationBooking.Contracts.Users
{
    public record UpdatePersonalDetailsRequest(
        string FirstName,
        string LastName,
        string Phone);
}