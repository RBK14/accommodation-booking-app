using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.UpdateReservationStatus
{
    public class UpdateReservationStatusCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateReservationStatusCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(UpdateReservationStatusCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Reservations.GetByIdAsync(command.ReservationId, cancellationToken) is not Reservation reservation)
                return Errors.Reservation.NotFound;

            var newStatus = ReservationStatusExtensions.Parse(command.Status);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                switch (newStatus)
                {
                    case ReservationStatus.InProgress:
                        reservation.MarkAsInProgress();
                        break;

                    case ReservationStatus.Completed:
                        reservation.MarkAsCompleted();
                        break;

                    case ReservationStatus.Cancelled:
                        reservation.Cancel();
                        break;

                    case ReservationStatus.NoShow:
                        reservation.MarkAsNoShow();
                        break;

                    case ReservationStatus.Accepted:
                        return Error.Validation(
                            "Reservation.InvalidTransition",
                            "Cannot manually transition to 'Accepted' status.");

                }

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            // TODO: Przenieść Errors do Domain
            catch (DomainIllegalStateException ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Conflict("Reservation.IllegalState", ex.Message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Failure("Reservation.UpdateFailed", ex.Message);
            }

            return Unit.Value;
        }
    }
}
