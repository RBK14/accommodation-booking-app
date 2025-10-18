namespace AccommodationBooking.Application.Common.Intrefaces.Authentication
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool Verify(string password, string passwordHash);
    }
}
