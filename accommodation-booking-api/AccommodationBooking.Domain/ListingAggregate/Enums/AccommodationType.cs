using AccommodationBooking.Domain.Common.Exceptions;

namespace AccommodationBooking.Domain.ListingAggregate.Enums
{
    public enum AccommodationType
    {
        Apartment,
        House,
        Hotel
    }

    public static class AccommodationTypeExtensions
    {
        public static bool TryParseAccommodationType(string? value, out AccommodationType type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                type = default;
                return false;
            }

            return Enum.TryParse(value, ignoreCase: true, out type)
                   && Enum.IsDefined(typeof(AccommodationType), type);
        }

        public static AccommodationType ParseAccommodationType(string value)
        {
            if (!TryParseAccommodationType(value, out var type))
                throw new DomainValidationException($"Invalid accommodation type: {value}");

            return type;
        }

        public static bool IsValidAccommodationType(string? value)
        {
            return TryParseAccommodationType(value, out _);
        }
    }
}
