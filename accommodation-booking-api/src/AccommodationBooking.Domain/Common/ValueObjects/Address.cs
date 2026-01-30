using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.Common.ValueObjects
{
    /// <summary>
    /// Represents a physical address.
    /// </summary>
    public sealed class Address : ValueObject
    {
        public string Country { get; init; }
        public string City { get; init; }
        public string PostalCode { get; init; }
        public string Street { get; init; }
        public string BuildingNumber { get; init; }
    
        private Address(
            string country,
            string city,
            string postalCode,
            string street,
            string buildingNumber)
        {
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            BuildingNumber = buildingNumber;
        }

        /// <summary>
        /// Creates a new address instance.
        /// </summary>
        public static Address Create(
            string country,
            string city,
            string postalCode,
            string street,
            string buildingNumber)
        {
            return new Address(
                country,
                city,
                postalCode,
                street,
                buildingNumber);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return PostalCode;
            yield return Street;
            yield return BuildingNumber;
        }

#pragma warning disable CS8618
        private Address()
        {
        }
#pragma warning restore CS8618
    }
}
