namespace AccommodationBooking.Domain.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation violates the expected domain state.
    /// </summary>
    public class DomainIllegalStateException : DomainException
    {
        public DomainIllegalStateException(string message) : base(message) { }
    }
}
