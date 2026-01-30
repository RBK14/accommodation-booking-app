using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteGuest
{
    public class DeleteGuestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGuestCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteGuestCommand command, CancellationToken cancellationToken)
        {
            // Walidacja
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            if (await _unitOfWork.GuestProfiles.GetByUserIdAsync(user.Id, cancellationToken) is not GuestProfile guestProfile)
                return Errors.GuestProfile.NotFound;

            if (user.Id != guestProfile.UserId)
                return Error.Conflict(
                    "User.InvalidProfile",
                    "Nie mozesz usunac tego uzytownika, poniewaz profil goscia jest nieprawidlowy");

            // Pobranie rezerwacji
            var guestReservations = await _unitOfWork.Reservations.SearchAsync(
                new List<IFilterable<Reservation>> { new Reservations.Common.GuestProfileIdFilter(guestProfile.Id) },
                cancellationToken);

            // Wybranie aktywnych rezerwacji
            var acceptedReservations = guestReservations
                .Where(r => r.Status == ReservationStatus.Accepted)
                .ToList();

            var listingIdsToFetch = acceptedReservations
                .Select(r => r.ListingId)
                .Distinct()
                .ToList();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                IEnumerable<Listing> listings = new List<Listing>();

                if (listingIdsToFetch.Count > 0)
                {
                    // Pobranie ofert dla aktywnych rezerwacji
                    listings = await _unitOfWork.Listings.SearchAsync(
                        new List<IFilterable<Listing>> { new Listings.Common.ListingIdsFilter(listingIdsToFetch) },
                        cancellationToken);
                }

                // Zwolnienie terminów i usuniecie rezerwacji
                foreach (var reservation in guestReservations)
                {
                    if (reservation.Status == ReservationStatus.Accepted)
                    {
                        var listing = listings.FirstOrDefault(l => l.Id == reservation.ListingId);
                        listing?.CancelReservation(reservation.Id);
                    }
                }
                _unitOfWork.Reservations.RemoveRange(guestReservations);


                _unitOfWork.GuestProfiles.Remove(guestProfile);
                _unitOfWork.Users.Remove(user);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Errors.User.DeleteFailed;
            }

            return Unit.Value;
        }
    }
}
