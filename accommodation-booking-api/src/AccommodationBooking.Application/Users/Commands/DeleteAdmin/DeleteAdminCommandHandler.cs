using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteAdmin
{
    public class DeleteAdminCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAdminCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteAdminCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            try
            {
                _unitOfWork.Users.Remove(user);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.User.DeleteFailed;
            }

            return Unit.Value;
        }
    }
}
