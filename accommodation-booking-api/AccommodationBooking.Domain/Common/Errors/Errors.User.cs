using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(
                code: "User.DuplicateEmail",
                description: "Użytownik z takim adresem email już istnieje.");

            public static Error NotFound => Error.NotFound(
                code: "User.NotFound",
                description: "Nie znaleziono żadnego użytownika spełniającego wymagania");

            public static Error CreationFailed => Error.Failure(
                code: "User.CreationFailed",
                description: "Nie udało się utworzyć użytkownika.");

            public static Error UpdateFailed => Error.Failure(
                code: "User.UpdateFailed",
                description: "Nie udało się zaktualizować użytkownika.");
        }
    }
}
