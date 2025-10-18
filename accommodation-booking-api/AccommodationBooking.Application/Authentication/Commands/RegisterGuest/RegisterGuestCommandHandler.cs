using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Users;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterGuest
{
    public class RegisterGuestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RegisterGuestCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(RegisterGuestCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email;
            if (await _unitOfWork.Users.GetByEmailAsync(email) is not null)
                return Errors.User.DuplicateEmail;

            var passwordHash = command.Password;

            var user = User.CreateGuest(
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
