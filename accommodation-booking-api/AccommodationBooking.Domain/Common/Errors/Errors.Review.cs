using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Review
        {
            public static Error NotFound => Error.NotFound(
                code: "Review.NotFound",
                description: "Nie znaleziono żadnej opinii spełniającej wymagania.");
        }
    }
}
