using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Reservations.Commands.CreateReservation;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.ReservationAggregate;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.CreateReservation
{
    public class CreateReservationCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly IGuestProfileRepository _guestRepositoryMock;
        private readonly IReservationRepository _reservationRepositoryMock;
        private readonly CreateReservationCommandHandler _handler;

        public CreateReservationCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();
            _guestRepositoryMock = Substitute.For<IGuestProfileRepository>();
            _reservationRepositoryMock = Substitute.For<IReservationRepository>();

            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);
            _unitOfWorkMock.GuestProfiles.Returns(_guestRepositoryMock);
            _unitOfWorkMock.Reservations.Returns(_reservationRepositoryMock);

            _handler = new CreateReservationCommandHandler(_unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_CreateReservation_When_DataIsValid()
        {
            var command = CreateCommand();
            var listing = CreateValidListing();
            var guest = GuestProfile.Create(Guid.NewGuid());

            _listingRepositoryMock.GetByIdAsync(command.ListingId, Arg.Any<CancellationToken>())
                .Returns(listing);
            _guestRepositoryMock.GetByIdAsync(command.GuestProfileId, Arg.Any<CancellationToken>())
                .Returns(guest);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Reservation>();

            listing.ReservationIds.Should().Contain(result.Value.Id);
            guest.ReservationIds.Should().Contain(result.Value.Id);

            _reservationRepositoryMock.Received(1).Add(Arg.Any<Reservation>());
            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundError_When_ListingDoesNotExist()
        {
            var command = CreateCommand();
            _listingRepositoryMock.GetByIdAsync(command.ListingId, Arg.Any<CancellationToken>())
                .Returns((Listing?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Listing.NotFound);

            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundError_When_GuestDoesNotExist()
        {
            var command = CreateCommand();
            var listing = CreateValidListing();

            _listingRepositoryMock.GetByIdAsync(command.ListingId, Arg.Any<CancellationToken>())
                .Returns(listing);
            _guestRepositoryMock.GetByIdAsync(command.GuestProfileId, Arg.Any<CancellationToken>())
                .Returns((GuestProfile?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.GuestProfile.NotFound);
        }

        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_DatesOverlap()
        {
            var command = CreateCommand();
            var listing = CreateValidListing();
            var guest = GuestProfile.Create(Guid.NewGuid());

            var start = command.CheckIn.ToDateTime(new TimeOnly(13, 0));
            var end = command.CheckOut.ToDateTime(new TimeOnly(9, 0));

            listing.ReserveDates(Guid.NewGuid(), start, end);

            _listingRepositoryMock.GetByIdAsync(command.ListingId, Arg.Any<CancellationToken>())
                .Returns(listing);
            _guestRepositoryMock.GetByIdAsync(command.GuestProfileId, Arg.Any<CancellationToken>())
                .Returns(guest);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Reservation.CreationFailed);

            await _unitOfWorkMock.Received(1).RollbackAsync(Arg.Any<CancellationToken>());

            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        private static CreateReservationCommand CreateCommand()
        {
            return new CreateReservationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(12))
            );
        }

        private static Listing CreateValidListing()
        {
            return Listing.Create(
                Guid.NewGuid(),
                "Title",
                "Desc",
                AccommodationType.Apartment,
                2,
                4,
                "PL", "Waw", "00-000", "Str", "1",
                100,
                Currency.PLN);
        }
    }
}