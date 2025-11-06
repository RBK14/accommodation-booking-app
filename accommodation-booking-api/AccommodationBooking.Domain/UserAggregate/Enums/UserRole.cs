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
            if (string.IsNullOrWhiteSpace(roleValue))
            {
                role = default;
                return false;
            }

            return Enum.TryParse(roleValue, ignoreCase: true, out role)
                   && Enum.IsDefined(typeof(UserRole), role);
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
