using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterAdmin
{
    public class RegisterAdminCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<RegisterAdminCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<ErrorOr<Unit>> Handle(RegisterAdminCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email;
            if (await _unitOfWork.Users.GetByEmailAsync(email) is not null)
                return Errors.User.DuplicateEmail;

            var passwordHash = _passwordHasher.HashPassword(command.Password);

            var user = User.CreateAdmin(
            email: email,
            passwordHash: passwordHash,
            firstName: command.FirstName,
            lastName: command.LastName,
            phone: command.Phone);

            try
            {
                _unitOfWork.Users.Add(user);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.User.CreationFailed;
            }

            return Unit.Value;
        }
    }
}
