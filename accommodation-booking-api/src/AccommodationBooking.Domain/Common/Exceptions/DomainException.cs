namespace AccommodationBooking.Domain.Common.Exceptions
{
    /// <summary>
    /// Base exception for domain-level errors.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
