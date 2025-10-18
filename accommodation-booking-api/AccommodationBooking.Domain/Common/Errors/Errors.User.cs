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

            public static Error CreationFailed => Error.Failure(
                code: "User.CreationFailed",
                description: "Nie udało się utworzyć użytkownika.");
        }
    }
}
