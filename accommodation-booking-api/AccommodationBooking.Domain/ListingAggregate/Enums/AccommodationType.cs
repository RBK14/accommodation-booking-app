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
        public static AccommodationType ParseAccommodationType(string? accommodationTypeValue)
        {
            Enum.TryParse(accommodationTypeValue, ignoreCase: true, out AccommodationType accommodationType);

            return accommodationType;
        }

        public static bool TryParseAccommodationType(string? accommodationTypeValue, out AccommodationType accommodationType)
        {
            return Enum.TryParse(accommodationTypeValue, ignoreCase: true, out accommodationType);
        }


        public static bool IsValidAccommodationType(string? accommodationType)
        {
            return TryParseAccommodationType(accommodationType, out _);
        }
    }
}
