using AccommodationBooking.Domain.Common.Exceptions;

namespace AccommodationBooking.Domain.ListingAggregate.Enums
{
    public enum AccommodationType
    {
        Apartment,
        House,
        Hotel,
        Villa,
        Room,
        Loft,
        Studio
    }

    public static class AccommodationTypeExtensions
    {
        public static bool TryParse(string? value, out AccommodationType type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                type = default;
                return false;
            }

            return Enum.TryParse(value, ignoreCase: true, out type)
                   && Enum.IsDefined(typeof(AccommodationType), type);
        }

        public static AccommodationType Parse(string value)
        {
            if (!TryParse(value, out var type))
                throw new DomainValidationException($"Invalid accommodation type: {value}");

            return type;
        }

        public static bool IsValidAccommodationType(string? value)
        {
            return TryParse(value, out _);
        }
    }
}
