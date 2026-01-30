namespace AccommodationBooking.Domain.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when domain validation rules are violated.
    /// </summary>
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
