using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Users.Commands.DeleteHost;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.DeleteUser
{
    public class DeleteHostCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IHostProfileRepository _hostProfileRepositoryMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly IReservationRepository _reservationRepositoryMock;
        private readonly DeleteHostCommandHandler _handler;

        public DeleteHostCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _hostProfileRepositoryMock = Substitute.For<IHostProfileRepository>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();
            _reservationRepositoryMock = Substitute.For<IReservationRepository>();

            _unitOfWorkMock.Users.Returns(_userRepositoryMock);
            _unitOfWorkMock.HostProfiles.Returns(_hostProfileRepositoryMock);
            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);
            _unitOfWorkMock.Reservations.Returns(_reservationRepositoryMock);

            _handler = new DeleteHostCommandHandler(_unitOfWorkMock);
        }

        private static Address CreateValidAddress() =>
            Address.Create("Poland", "Warsaw", "Mazowieckie", "00-110", "ul. Marszałkowska 123");

        private static Price CreateValidPrice(decimal amount = 100) =>
            Price.Create(amount, Currency.PLN);

        [Fact]
        public async Task Handle_Should_DeleteUserHostListingsAndReservations_When_NoActiveReservationsExist()
        {
            var userId = Guid.NewGuid();
            var command = new DeleteHostCommand(userId);

            var user = User.CreateHost("test@test.com", "pass", "F", "L", "123");
            SetId(user, userId);
            var hostProfile = HostProfile.Create(userId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _hostProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(hostProfile);

            var listing = CreateListing(hostProfile.Id);
            var listings = new List<Listing> { listing };
            var address = CreateValidAddress();
            var price = CreateValidPrice();

            _listingRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Listing>>>(), Arg.Any<CancellationToken>())
                .Returns(listings);

            var reservation = Reservation.Create(listing.Id, Guid.NewGuid(), hostProfile.Id, "T", address, price, DateTime.Now, DateTime.Now.AddDays(1));
            SetStatus(reservation, ReservationStatus.Cancelled);
            var reservations = new List<Reservation> { reservation };

            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(reservations);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();

            _reservationRepositoryMock.Received(1).RemoveRange(Arg.Is<IEnumerable<Reservation>>(x => x.SequenceEqual(reservations)));
            _hostProfileRepositoryMock.Received(1).Remove(hostProfile);
            _userRepositoryMock.Received(1).Remove(user);

            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnConflictError_When_ActiveReservationsExist()
        {
            var userId = Guid.NewGuid();
            var command = new DeleteHostCommand(userId);

            var user = User.CreateHost("t", "p", "f", "l", "1");
            SetId(user, userId);
            var hostProfile = HostProfile.Create(userId);
            var listing = CreateListing(hostProfile.Id);
            var address = CreateValidAddress();
            var price = CreateValidPrice();

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _hostProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(hostProfile);
            _listingRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Listing>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Listing> { listing });

            var activeReservation = Reservation.Create(listing.Id, Guid.NewGuid(), hostProfile.Id, "T", address, price, DateTime.Now, DateTime.Now.AddDays(1));
            SetStatus(activeReservation, ReservationStatus.Accepted);

            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Reservation> { activeReservation });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("User.CannotDeleteActive");

            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
            _userRepositoryMock.DidNotReceive().Remove(Arg.Any<User>());
            _listingRepositoryMock.DidNotReceive().RemoveRange(Arg.Any<IEnumerable<Listing>>());
        }

        [Fact]
        public async Task Handle_Should_WorkCorrectly_When_HostHasNoListings()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateHost("t", "p", "f", "l", "1");
            SetId(user, userId);
            var hostProfile = HostProfile.Create(userId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _hostProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(hostProfile);

            _listingRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Listing>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Listing>());

            var result = await _handler.Handle(new DeleteHostCommand(userId), CancellationToken.None);

            result.IsError.Should().BeFalse();

            await _reservationRepositoryMock.DidNotReceive().SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>());

            _hostProfileRepositoryMock.Received(1).Remove(hostProfile);
            _userRepositoryMock.Received(1).Remove(user);
            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnUserNotFound_When_UserDoesNotExist()
        {
            _userRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var result = await _handler.Handle(new DeleteHostCommand(Guid.NewGuid()), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.NotFound);
        }

        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_ExceptionOccurs()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateHost("t", "p", "f", "l", "1");
            SetId(user, userId);
            var hostProfile = HostProfile.Create(userId);

            _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            _hostProfileRepositoryMock.GetByUserIdAsync(userId, Arg.Any<CancellationToken>()).Returns(hostProfile);
            _listingRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Listing>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Listing>());

            _hostProfileRepositoryMock.When(x => x.Remove(hostProfile)).Do(x => { throw new Exception("DB Fail"); });

            var result = await _handler.Handle(new DeleteHostCommand(userId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.DeleteFailed);

            await _unitOfWorkMock.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        private static Listing CreateListing(Guid hostId)
        {
            return Listing.Create(hostId, "Title", "Desc", AccommodationType.Apartment, 1, 1, "C", "C", "P", "S", "1", 100, Currency.USD);
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
                if (field != null) field.SetValue(reservation, status);
            }
        }
    }
}