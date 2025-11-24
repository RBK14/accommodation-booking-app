using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdatePassword
{
    public class UpdatePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher) : IRequestHandler<UpdatePasswordCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<ErrorOr<Unit>> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
                return Errors.Auth.InvalidPassword;

            var newPasswordHash = _passwordHasher.HashPassword(command.NewPassword);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.UpdatePasswordHash(newPasswordHash);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.User.UpdateFailed;
            }

            return Unit.Value;
        }
    }
}
