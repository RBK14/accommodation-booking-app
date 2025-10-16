using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.Common.ValueObjects
{
    public sealed class Address : ValueObject
    {
        string Country { get; init; }
        string City { get; init; }
        string PostalCode { get; init; }
        string Street { get; init; }
        string BuildingNumber { get; init; }
    
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
