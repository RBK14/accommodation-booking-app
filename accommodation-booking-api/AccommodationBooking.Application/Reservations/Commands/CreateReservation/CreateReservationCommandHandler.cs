using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReservationCommand, ErrorOr<Reservation>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Reservation>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var listingId = command.ListingId;
            if (await _unitOfWork.Listings.GetByIdAsync(listingId) is not Listing listing)
                return Errors.Listing.NotFound;

            var guestProfileId = command.GuestProfileId;
            if (await _unitOfWork.GuestProfiles.GetByIdAsync(guestProfileId) is not GuestProfile guest)
                return Errors.GuestProfile.NotFound;

            var checkIn = command.CheckIn;
            var checkOut = command.CheckOut;

            var reservation = Reservation.Create(
                listingId,
                guestProfileId,
                listing.HostProfileId,
                listing.Title,
                listing.Address,
                listing.PricePerDay,
                checkIn,
                checkOut);

            try
            {
                guest.AddReservationId(reservation.Id);
                listing.ReserveDates(reservation.Id, checkIn, checkOut);
                _unitOfWork.Reservations.Add(reservation);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.Reservation.CreationFailed;
            }

            return reservation;
        }
    }
}
