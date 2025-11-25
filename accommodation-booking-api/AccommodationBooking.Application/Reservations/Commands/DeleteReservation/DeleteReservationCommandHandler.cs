using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.DeleteReservation
{
    public class DeleteReservationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReservationCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteReservationCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Reservations.GetByIdAsync(command.ReservationId, cancellationToken) is not Reservation reservation)
                return Errors.Reservation.NotFound;

            if (await _unitOfWork.Listings.GetByIdAsync(reservation.ListingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            if (await _unitOfWork.GuestProfiles.GetByIdAsync(reservation.GuestProfileId, cancellationToken) is not GuestProfile guestProfile)
                return Errors.GuestProfile.NotFound;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                listing.CancelReservation(reservation.Id);

                guestProfile.RemoveReservationId(reservation.Id);

                _unitOfWork.Reservations.Remove(reservation);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure(
                    "Reservation.DeleteFailed",
                    "Nie udało się usunąc rezerwacji.");
            }

            return Unit.Value;
        }
    }
}
