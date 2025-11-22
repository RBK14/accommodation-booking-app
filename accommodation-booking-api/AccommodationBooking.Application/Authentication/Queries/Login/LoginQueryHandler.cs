using AccommodationBooking.Application.Authentication.Common;
using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.UserAggregate;
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
            if (await _unitOfWork.Users.GetByEmailAsync(email) is not User user)
                return Errors.Auth.InvalidCredentials;

            if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
                return Errors.Auth.InvalidCredentials;

            // TODO: Wyciągnąć ID profilu
            var profileId = Guid.Empty;

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, profileId);

            return new AuthResultDto(user, accessToken);
        }
    }
}
