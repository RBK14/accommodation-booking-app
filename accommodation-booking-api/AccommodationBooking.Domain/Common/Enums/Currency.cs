using AccommodationBooking.Domain.UserAggregate.Enums;

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
        public static Currency ParseCurrency(string? currencyValue)
        {
            Enum.TryParse(currencyValue, ignoreCase: true, out Currency currency);

            return currency;
        }

        public static bool TryParseCurrency(string? currencyValue, out Currency currency)
        {
            return Enum.TryParse(currencyValue, ignoreCase: true, out currency);
        }


        public static bool IsValidCurrency(string? currencyValue)
        {
            return TryParseCurrency(currencyValue, out _);
        }
    }
}
