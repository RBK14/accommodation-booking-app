using AccommodationBooking.Application.Authentication.Common;
using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Users;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<LoginQuery, ErrorOr<AuthResult>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<ErrorOr<AuthResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            var email = query.Email;
            if (await _unitOfWork.Users.GetByEmailAsync(email) is not User user)
                return Errors.Auth.InvalidCredentials;

            if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
                return Errors.Auth.InvalidCredentials;

            // TODO: Generowanie tokenu JWT

            return new AuthResult(user, default!);
        }
    }
}
