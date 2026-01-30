using AccommodationBooking.Application.Authentication.Common;
using AccommodationBooking.Application.Common.Interfaces.Authentication;
using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginQuery, ErrorOr<AuthResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        public async Task<ErrorOr<AuthResultDto>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            var email = query.Email;
            if (await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken) is not User user)
                return Errors.Auth.InvalidCredentials;

            if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
                return Errors.Auth.InvalidCredentials;

            var profileId = Guid.Empty;

            if (user.Role == UserRole.Guest)
            {
                if (await _unitOfWork.GuestProfiles.GetByUserIdAsync(user.Id, cancellationToken) is GuestProfile profile)
                    profileId = profile.Id;
            }
            else if (user.Role == UserRole.Host)
            {
                if (await _unitOfWork.HostProfiles.GetByUserIdAsync(user.Id, cancellationToken) is HostProfile profile)
                    profileId = profile.Id;
            }

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, profileId);

            return new AuthResultDto(user, accessToken);
        }
    }
}
