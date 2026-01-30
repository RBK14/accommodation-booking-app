namespace AccommodationBooking.Application.Common.Interfaces.Authentication
{
    /// <summary>
    /// Interface for password hashing operations.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes a password.
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        bool Verify(string password, string passwordHash);
    }
}
