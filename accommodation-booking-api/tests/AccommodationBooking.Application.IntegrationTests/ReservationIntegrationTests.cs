using AccommodationBooking.Application.Listings.Commands.DeleteListing;
using AccommodationBooking.Application.Reservations.Commands.CreateReservation;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate;
using FluentAssertions;

namespace AccommodationBooking.Application.IntegrationTests
{
    public class ReservationIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task Handle_Should_CreateReservation_When_DataIsCorrect()
        {
            using var dbContext = CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);
            var handler = new CreateReservationCommandHandler(unitOfWork);

            var host = User.CreateHost("host@test.com", "hash", "F", "L", "1");
            var guest = User.CreateGuest("guest@test.com", "hash", "F", "L", "2");
            dbContext.Users.AddRange(host, guest);

            var hostProfile = HostProfile.Create(host.Id);
            var guestProfile = GuestProfile.Create(guest.Id);
            dbContext.HostProfiles.Add(hostProfile);
            dbContext.GuestProfiles.Add(guestProfile);

            var listing = Listing.Create(
                hostProfile.Id,
                "Tytuł",
                "Opis",
                AccommodationType.Apartment,
                2,
                1,
                "Polska", "Wrocław", "50-000", "Rynek", "13",
                100, Currency.PLN);

            dbContext.Listings.Add(listing);

            await dbContext.SaveChangesAsync();

            var command = new CreateReservationCommand(
                listing.Id,
                guestProfile.Id,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(12))
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();

            var reservation = dbContext.Reservations.FirstOrDefault();
            reservation.Should().NotBeNull();
            reservation!.ListingId.Should().Be(listing.Id);
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_ListingDoesNotExist()
        {
            using var dbContext = CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);

            var handler = new CreateReservationCommandHandler(unitOfWork);

            var guest = User.CreateGuest("guest@test.com", "hash", "F", "L", "1");
            dbContext.Users.Add(guest);
            await dbContext.SaveChangesAsync();

            var command = new CreateReservationCommand(
                Guid.NewGuid(),
                guest.Id,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Listing.NotFound);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_ListingIsDeletedJustBefore()
        {
            using var dbContext = CreateDbContext();

            var unitOfWork = CreateUnitOfWork(dbContext);

            var deleteListingHandler = new DeleteListingCommandHandler(unitOfWork);
            var createReservationHandler = new CreateReservationCommandHandler(unitOfWork);

            var hostUser = User.CreateHost("host@test.com", "Password123!", "Host", "User", "1");
            var guestUser = User.CreateGuest("guest@test.com", "Password123!", "Guest", "User", "2");
            dbContext.Users.AddRange(hostUser, guestUser);

            var hostProfile = HostProfile.Create(hostUser.Id);
            var guestProfile = GuestProfile.Create(guestUser.Id);
            dbContext.HostProfiles.Add(hostProfile);
            dbContext.GuestProfiles.Add(guestProfile);

            var listing = Listing.Create(
                hostProfile.Id,
                "Tytuł",
                "Opis",
                AccommodationType.Apartment,
                2,
                1,
                "Polska", "Wroclaw", "50-000", "Rynek", "13",
                100, Currency.PLN);

            dbContext.Listings.Add(listing);

            hostProfile.AddListingId(listing.Id);

            await dbContext.SaveChangesAsync();

            var targetListingId = listing.Id;

            var deleteCommand = new DeleteListingCommand(targetListingId, hostProfile.Id);
            var deleteResult = await deleteListingHandler.Handle(deleteCommand, CancellationToken.None);

            deleteResult.IsError.Should().BeFalse("Host powinien móc usunąć swoją ofertę bez błędów");

            var reservationCommand = new CreateReservationCommand(
                targetListingId,
                guestUser.Id,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
            );

            var reservationResult = await createReservationHandler.Handle(reservationCommand, CancellationToken.None);

            reservationResult.IsError.Should().BeTrue();
            reservationResult.FirstError.Should().Be(Errors.Listing.NotFound);

            dbContext.Reservations.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Fail_When_DatesOverlapWithExistingReservation()
        {
            using var dbContext = CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);
            var handler = new CreateReservationCommandHandler(unitOfWork);

            var host = User.CreateHost("host@test.com", "hash", "F", "L", "1");
            var guest1 = User.CreateGuest("guest1@test.com", "hash", "F1", "L1", "2");
            var guest2 = User.CreateGuest("guest2@test.com", "hash", "F2", "L2", "3");
            dbContext.Users.AddRange(host, guest1, guest2);

            var hostProfile = HostProfile.Create(host.Id);
            var guestProfile1 = GuestProfile.Create(guest1.Id);
            var guestProfile2 = GuestProfile.Create(guest2.Id);
            dbContext.HostProfiles.Add(hostProfile);
            dbContext.GuestProfiles.AddRange(guestProfile1, guestProfile2);

            var listing = Listing.Create(
                hostProfile.Id,
                "Tytuł",
                "Opis",
                AccommodationType.Apartment,
                2,
                1,
                "Polska", "Wroclaw", "50-000", "Rynek", "13",
                100, Currency.PLN);

            dbContext.Listings.Add(listing);

            await dbContext.SaveChangesAsync();

            var year = DateTime.UtcNow.AddYears(1).Year;

            var command1 = new CreateReservationCommand(
                ListingId: listing.Id,
                guestProfile1.Id,
                new DateOnly(year, 1, 10),
                new DateOnly(year, 1, 12));

            await handler.Handle(command1, CancellationToken.None);

            dbContext.ChangeTracker.Clear();

            var command2 = new CreateReservationCommand(
                ListingId: listing.Id,
                guestProfile2.Id,
                new DateOnly(year, 1, 9),
                new DateOnly(year, 1, 14));

            var result = await handler.Handle(command2, CancellationToken.None);

            result.IsError.Should().BeTrue();
        }
    }
}
