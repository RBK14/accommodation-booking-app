using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Models;

namespace AccommodationBooking.Domain.Common.ValueObjects
{
    /// <summary>
    /// Represents a monetary value with currency.
    /// </summary>
    public sealed class Price : ValueObject
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }

        private Price(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        /// <summary>
        /// Creates a new price instance.
        /// </summary>
        public static Price Create(decimal amount, Currency currency)
        {
            return new Price(amount, currency);
        }

        /// <summary>
        /// Creates a copy of this price.
        /// </summary>
        public Price Copy()
        {
            return new Price(Amount, Currency);
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
