namespace AccommodationBooking.Domain.UserAggregate.Enums
{
    public enum UserRole
    {
        Guest,
        Host,
        Admin
    }

    public static class UserRoleExtensions
    {
        public static bool TryParseRole(string? roleValue, out UserRole role)
        {
            return Enum.TryParse(roleValue, ignoreCase: true, out role);
        }

        public static bool IsValidRole(string? roleValue)
        {
            return TryParseRole(roleValue, out _);
        }

        public static bool IsInRole(this string? roleValue, UserRole expectedRole)
        {
            return TryParseRole(roleValue, out var role) && role == expectedRole;
        }
    }
}
