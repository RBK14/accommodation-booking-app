using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Users.Commands.DeleteGuest;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate;
using ErrorOr;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.DeleteUser
{
    /// <summary>
    /// Unit tests for DeleteGuestCommandHandler.
    /// </summary>
    public class DeleteGuestCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IGuestProfileRepository _guestProfileRepositoryMock;
        private readonly IReservationRepository _reservationRepositoryMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly DeleteGuestCommandHandler _handler;

        public DeleteGuestCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _guestProfileRepositoryMock = Substitute.For<IGuestProfileRepository>();
            _reservationRepositoryMock = Substitute.For<IReservationRepository>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();

            _unitOfWorkMock.Users.Returns(_userRepositoryMock);
            _unitOfWorkMock.GuestProfiles.Returns(_guestProfileRepositoryMock);
            _unitOfWorkMock.Reservations.Returns(_reservationRepositoryMock);
            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);

            _handler = new DeleteGuestCommandHandler(_unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_DeleteUserGuestAndReservations_When_ProfileIsValid()
        {
            var command = new DeleteGuestCommand(Guid.NewGuid());

            var user = User.CreateGuest("test@test.com", "hash", "F", "L", "123");
            SetId(user, command.UserId);

            var guestProfile = GuestProfile.Create(command.UserId);

            _userRepositoryMock.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);
            _guestProfileRepositoryMock.GetByUserIdAsync(command.UserId, Arg.Any<CancellationToken>())
                .Returns(guestProfile);

            var listing = CreateListing();
            var address = Address.Create("Poland", "Krakow", "30-001", "Rynek Główny", "1");
            var price = Price.Create(100, Currency.PLN);
            var checkIn = DateTime.UtcNow.AddDays(1);
            var checkOut = DateTime.UtcNow.AddDays(3);

            var r1 = Reservation.Create(listing.Id, guestProfile.Id, listing.HostProfileId, "Title", address, price, checkIn, checkOut);
            SetStatus(r1, ReservationStatus.Accepted);
            listing.ReserveDates(r1.Id, checkIn, checkOut);

            var r2 = Reservation.Create(listing.Id, guestProfile.Id, listing.HostProfileId, "Title", address, price, DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(11));
            SetStatus(r2, ReservationStatus.Cancelled);

            var reservations = new List<Reservation> { r1, r2 };

            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(reservations);

            _listingRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Listing>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Listing> { listing });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();

            _reservationRepositoryMock.Received(1).RemoveRange(reservations);
            _guestProfileRepositoryMock.Received(1).Remove(guestProfile);
            _userRepositoryMock.Received(1).Remove(user);

            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnUserNotFound_When_UserDoesNotExist()
        {
            _userRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var result = await _handler.Handle(new DeleteGuestCommand(Guid.NewGuid()), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnGuestProfileNotFound_When_ProfileDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateGuest("a", "b", "c", "d", "e");
            SetId(user, userId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _guestProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns((GuestProfile?)null);

            var result = await _handler.Handle(new DeleteGuestCommand(userId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.GuestProfile.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnConflict_When_UserIdDoesNotMatchProfile()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var user = User.CreateGuest("a", "b", "c", "d", "e");
            SetId(user, userId);

            var guestProfile = GuestProfile.Create(otherUserId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _guestProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(guestProfile);

            var result = await _handler.Handle(new DeleteGuestCommand(userId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Conflict);
        }

        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_ExceptionOccursDuringDeletion()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateGuest("a", "b", "c", "d", "e");
            SetId(user, userId);
            var guestProfile = GuestProfile.Create(userId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _guestProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(guestProfile);
            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Reservation>());

            _userRepositoryMock.When(x => x.Remove(user)).Do(x => { throw new Exception("Db Error"); });

            var result = await _handler.Handle(new DeleteGuestCommand(userId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.DeleteFailed);

            await _unitOfWorkMock.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        private static Listing CreateListing()
        {
            return Listing.Create(Guid.NewGuid(), "T", "D", AccommodationType.Apartment, 1, 1, "C", "C", "P", "S", "1", 10, Currency.PLN);
        }

        private static void SetId<T>(AggregateRoot<T> aggregate, T id)
        {
            var prop = aggregate.GetType().BaseType!.GetProperty("Id");
            prop!.SetValue(aggregate, id);
        }

        private static void SetStatus(Reservation reservation, ReservationStatus status)
        {
            var prop = typeof(Reservation).GetProperty(nameof(Reservation.Status));
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(reservation, status);
            }
            else
            {
                var field = typeof(Reservation).GetField($"<{nameof(Reservation.Status)}>k__BackingField",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (field != null)
                    field.SetValue(reservation, status);
            }
        }
    }
}