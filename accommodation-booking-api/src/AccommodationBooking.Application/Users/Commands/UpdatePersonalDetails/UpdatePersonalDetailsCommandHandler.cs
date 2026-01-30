using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.UpdatePersonalDetails
{
    /// <summary>
    /// Handler for updating user's personal details.
    /// </summary>
    public class UpdatePersonalDetailsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalDetailsCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(UpdatePersonalDetailsCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            user.UpdatePersonalDetails(command.FirstName, command.LastName, command.Phone);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
