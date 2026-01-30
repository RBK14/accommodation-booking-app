using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserQuery, ErrorOr<User>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<User>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.GetByIdAsync(query.UserId) is not User user)
                return Errors.User.NotFound;

            return user;
        }
    }
}
