namespace AccommodationBooking.Domain.Common.Exceptions
{
    public class DomainIllegalStateException : DomainException
    {
        public DomainIllegalStateException(string message) : base(message) { }
    }
}
