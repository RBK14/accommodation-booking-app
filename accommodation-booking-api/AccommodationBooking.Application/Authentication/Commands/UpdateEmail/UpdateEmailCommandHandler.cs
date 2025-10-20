using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Users;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmailCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ErrorOr<Unit>> Handle(UpdateEmailCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.Id) is not User user)
                return Errors.User.NotFound;

            try
            {
                user.UpdateEmail(command.Email);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.User.UpdateFailed;
            }

            return Unit.Value;
        }
    }
}
