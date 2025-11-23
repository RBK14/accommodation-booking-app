using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class GuestProfile
        {
            public static Error NotFound => Error.NotFound(
                code: "GuestProfile.NotFound",
                description: "Nie znaleziono żadnego profilu gościa spełniającego wymagania.");
        }
    }
}
