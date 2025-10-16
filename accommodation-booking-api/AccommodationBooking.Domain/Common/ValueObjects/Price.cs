using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.Common.ValueObjects
{
    public sealed class Price : ValueObject
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }

        private Price(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Price Create(decimal amount, Currency currency)
        {
            return new Price(amount, currency);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

#pragma warning disable CS8618
        private Price()
        {
        }
#pragma warning restore CS8618
    }
}
