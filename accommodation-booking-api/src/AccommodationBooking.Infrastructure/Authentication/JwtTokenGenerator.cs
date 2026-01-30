using AccommodationBooking.Application.Common.Interfaces.Authentication;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccommodationBooking.Infrastructure.Authentication
{
    /// <summary>
    /// Generates JWT access tokens for authenticated users.
    /// </summary>
    public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        /// <summary>
        /// Generates a JWT access token for the specified user.
        /// </summary>
        public string GenerateAccessToken(User user, Guid profileId)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (user.Role == UserRole.Guest)
                claims.Add(new Claim("ProfileId", profileId.ToString()));
            else if (user.Role == UserRole.Host)
                claims.Add(new Claim("ProfileId", profileId.ToString()));

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
