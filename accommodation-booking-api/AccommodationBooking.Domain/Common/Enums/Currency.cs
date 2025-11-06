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
        public static bool TryParseCurrency(string? currencyValue, out Currency currency)
        {
            if (string.IsNullOrWhiteSpace(currencyValue))
            {
                currency = default;
                return false;
            }

            return Enum.TryParse(currencyValue, ignoreCase: true, out currency)
                   && Enum.IsDefined(typeof(Currency), currency);
        }

        public static Currency ParseCurrency(string currencyValue)
        {
            if (!TryParseCurrency(currencyValue, out var currency))
                throw new DomainValidationException($"Invalid currency value: {currencyValue}");

            return currency;
        }

        public static bool IsValidCurrency(string? currencyValue)
        {
            return TryParseCurrency(currencyValue, out _);
        }
    }
}
