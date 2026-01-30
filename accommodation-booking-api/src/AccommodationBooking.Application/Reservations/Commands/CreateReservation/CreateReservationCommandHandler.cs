using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.CreateReservation
{
    /// <summary>
    /// Handler for creating new reservations.
    /// </summary>
    public class CreateReservationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReservationCommand, ErrorOr<Reservation>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Reservation>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var listingId = command.ListingId;
            if (await _unitOfWork.Listings.GetByIdAsync(listingId, cancellationToken) is not Listing listing)
                return Errors.Listing.NotFound;

            var guestProfileId = command.GuestProfileId;
            if (await _unitOfWork.GuestProfiles.GetByIdAsync(guestProfileId, cancellationToken) is not GuestProfile guest)
                return Errors.GuestProfile.NotFound;

            var checkIn = command.CheckIn.ToDateTime(new TimeOnly(13, 0));
            var checkOut = command.CheckOut.ToDateTime(new TimeOnly(9, 0));

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            Reservation reservation;

            try
            {
                var addressCopy = listing.Address.Copy();
                var priceCopy = listing.PricePerDay.Copy();

                reservation = Reservation.Create(
                    listingId,
                    guestProfileId,
                    listing.HostProfileId,
                    listing.Title,
                    addressCopy,
                    priceCopy,
                    checkIn,
                    checkOut);

                guest.AddReservationId(reservation.Id);
                listing.ReserveDates(reservation.Id, checkIn, checkOut);
                _unitOfWork.Reservations.Add(reservation);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DomainValidationException ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Error.Validation("Reservation.ValidationFailed", ex.Message);
            }
            catch (DomainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Reservation.CreationFailed;
            }

            return reservation;
        }
    }
}
