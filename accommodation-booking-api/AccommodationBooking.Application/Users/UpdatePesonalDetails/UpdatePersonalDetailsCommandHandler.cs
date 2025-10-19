using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.UpdatePesonalDetails
{
    public class UpdatePersonalDetailsCommandHandler(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(UpdatePersonaDetailsCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(command.Id);
            if (user is null)
                return Error.NotFound();

            try 
            {
                user.UpdatePersonalDetails(command.FirstName, command.LastName, command.Phone);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure();
            }

            return Unit.Value;
        }
    }
}