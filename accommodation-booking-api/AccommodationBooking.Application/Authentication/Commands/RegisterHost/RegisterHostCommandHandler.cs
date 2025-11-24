using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterHost
{
    public class RegisterHostCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher) : IRequestHandler<RegisterHostCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        public async Task<ErrorOr<Unit>> Handle(RegisterHostCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email;
            if (await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken) is not null)
                return Errors.User.DuplicateEmail;

            var passwordHash = _passwordHasher.HashPassword(command.Password);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = User.CreateHost(
                email,
                passwordHash,
                command.FirstName,
                command.LastName,
                command.Phone);

                var profile = HostProfile.Create(user.Id);

                _unitOfWork.Users.Add(user);
                _unitOfWork.HostProfiles.Add(profile);
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
