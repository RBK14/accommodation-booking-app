using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Listing
        {
            public static Error NotFound => Error.NotFound(
                code: "Listing.NotFound",
                description: "Nie znaleziono żadnej oferty spełniającej wymagania.");

            public static Error CreationFailed => Error.Failure(
                code: "Listing.CreationFailed",
                description: "Nie udało się utworzyć oferty.");

            public static Error UpdateFailed => Error.Failure(
                code: "Listing.UpdateFailed",
                description: "Nie udało się zaktualizować oferty.");
        }
    }
}
