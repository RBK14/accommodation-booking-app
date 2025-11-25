using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Users.Commands.DeleteHost
{
    public class DeleteHostCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteHostCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteHostCommand command, CancellationToken cancellationToken)
        {
            // Walidacja
            if (await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken) is not User user)
                return Errors.User.NotFound;

            if (await _unitOfWork.HostProfiles.GetByIdAsync(command.HostProfileId, cancellationToken) is not HostProfile hostProfile)
                return Errors.GuestProfile.NotFound;

            if (user.Id != hostProfile.UserId)
                return Error.Conflict(
                    "User.InvalidProfile",
                    "Nie możesz usunąc tego użytownika, ponieważ profil gospodarza jest nieprawidłowy");

            // Pobranie ofert utworzonych przez użytkownika
            var listings = await _unitOfWork.Listings.SearchAsync(
                    new List<IFilterable<Listing>> { new Listings.Common.HostProfileIdFilter(hostProfile.Id) },
                    cancellationToken);

            var listingIds = listings.Select(l => l.Id).ToList();


            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                List<Reservation> reservationsToDelete = new();

                // Sprawdzenie, czy gospodarz posiada oferty z nadchodzącymi lub trwającymi rezerwacjami.
                if (listingIds.Count > 0)
                {
                    var listingReservations = await _unitOfWork.Reservations.SearchAsync(
                        new List<IFilterable<Reservation>> { new Reservations.Common.ListingIdsFilter(listingIds) },
                        cancellationToken);

                    bool hasActiveReservations = listingReservations.Any(r =>
                        r.Status == ReservationStatus.Accepted ||
                        r.Status == ReservationStatus.InProgress);

                    if (hasActiveReservations)
                    {
                        return Error.Conflict(
                            "User.CannotDeleteActive",
                            "Nie można usunąć użytkownika, który posiada posiadaja nadchodzące lub trwające rezerwacje");
                    }

                    reservationsToDelete.AddRange(listingReservations);
                }

                _unitOfWork.Reservations.RemoveRange(reservationsToDelete);

                _unitOfWork.Listings.RemoveRange(listings);

                _unitOfWork.HostProfiles.Remove(hostProfile);
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
