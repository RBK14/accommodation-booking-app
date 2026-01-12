using ErrorOr;

namespace AccommodationBooking.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class HostProfile
        {
            public static Error NotFound => Error.NotFound(
                code: "HostProfile.NotFound",
                description: "Nie znaleziono żadnego profilu gospodarza spełniającego wymagania.");
        }
    }
}
