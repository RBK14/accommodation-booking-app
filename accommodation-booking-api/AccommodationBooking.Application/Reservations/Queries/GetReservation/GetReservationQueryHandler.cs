using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Queries.GetReservation
{
    public class GetReservationQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReservationQuery, ErrorOr<Reservation>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Reservation>> Handle(GetReservationQuery query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Reservations.GetByIdAsync(query.ReservationId) is not Reservation reservation)
                return Errors.Reservation.NotFound;

            return reservation;
        }
    }
}
