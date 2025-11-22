using AccommodationBooking.Domain.Common.Exceptions;

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
        public static bool TryParse(string? roleValue, out UserRole role)
        {
            if (string.IsNullOrWhiteSpace(roleValue))
            {
                role = default;
                return false;
            }

            return Enum.TryParse(roleValue, ignoreCase: true, out role)
                   && Enum.IsDefined(typeof(UserRole), role);
        }

        public static UserRole Parse(string value)
        {
            if (!TryParse(value, out var role))
                throw new DomainValidationException($"Invalid user role: {value}");

            return role;
        }

        public static bool IsValidRole(string? roleValue)
        {
            return TryParse(roleValue, out _);
        }

        public static bool IsInRole(this string? roleValue, UserRole expectedRole)
        {
            return TryParse(roleValue, out var role) && role == expectedRole;
        }
    }
}
