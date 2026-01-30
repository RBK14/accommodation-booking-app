using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmailCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ErrorOr<Unit>> Handle(UpdateEmailCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            if (await _unitOfWork.Users.GetByEmailAsync(command.Email, cancellationToken) is not null)
                return Errors.User.DuplicateEmail;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.UpdateEmail(command.Email);
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
