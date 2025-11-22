using AccommodationBooking.Domain.Common.Exceptions;

namespace AccommodationBooking.Domain.Common.Enums
{
    public enum Currency
    {
        PLN,
        EUR,
        USD
    }

    public static class CurrencyExtensions
    {
        public static bool TryParse(string? currencyValue, out Currency currency)
        {
            if (string.IsNullOrWhiteSpace(currencyValue))
            {
                currency = default;
                return false;
            }

            return Enum.TryParse(currencyValue, ignoreCase: true, out currency)
                   && Enum.IsDefined(typeof(Currency), currency);
        }

        public static Currency Parse(string currencyValue)
        {
            if (!TryParse(currencyValue, out var currency))
                throw new DomainValidationException($"Invalid currency value: {currencyValue}");

            return currency;
        }

        public static bool IsValidCurrency(string? currencyValue)
        {
            return TryParse(currencyValue, out _);
        }
    }
}
