using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails
{
    public class UpdatePersonalDetailsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalDetailsCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(UpdatePersonalDetailsCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;
            

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try 
            {
                user.UpdatePersonalDetails(command.FirstName, command.LastName, command.Phone);
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