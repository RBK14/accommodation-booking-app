using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Reservation
        {
            public static Error NotFound => Error.NotFound(
                code: "Reservation.NotFound",
                description: "Nie znaleziono żadnej rezerwacji spełniającej wymagania.");

            public static Error CreationFailed => Error.Failure(
                code: "Reservation.CreationFailed",
                description: "Nie udało się utworzyć rezerwacji.");

            public static Error UpdateFailed => Error.Failure(
                code: "Reservation.UpdateFailed",
                description: "Nie udało się zaktualizować rezerwacji.");
        }
    }
}
