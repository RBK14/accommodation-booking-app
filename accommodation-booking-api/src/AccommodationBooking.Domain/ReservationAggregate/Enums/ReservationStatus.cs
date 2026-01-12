using AccommodationBooking.Domain.Common.Exceptions;

namespace AccommodationBooking.Domain.ReservationAggregate.Enums
{
    public enum ReservationStatus
    {
        Accepted,
        InProgress,
        Completed,
        Cancelled,
        NoShow
    }
    public static class ReservationStatusExtensions
    {
        public static bool TryParse(string? value, out ReservationStatus status)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                status = default;
                return false;
            }

            return Enum.TryParse(value, ignoreCase: true, out status)
                   && Enum.IsDefined(typeof(ReservationStatus), status);
        }

        public static ReservationStatus Parse(string value)
        {
            if (!TryParse(value, out var status))
                throw new DomainValidationException($"Invalid reservation status: {value}");

            return status;
        }

        public static bool IsValidReservationStatus(string? value)
        {
            return TryParse(value, out _);
        }
    }
}


