using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Listings.Commands.DeleteListing;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.Common.Models;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using ErrorOr;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.DeleteListing
{
    public class DeleteListingCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly IReservationRepository _reservationRepositoryMock;
        private readonly IHostProfileRepository _hostProfileRepositoryMock;
        private readonly DeleteListingCommandHandler _handler;

        public DeleteListingCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();
            _reservationRepositoryMock = Substitute.For<IReservationRepository>();
            _hostProfileRepositoryMock = Substitute.For<IHostProfileRepository>();

            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);
            _unitOfWorkMock.Reservations.Returns(_reservationRepositoryMock);
            _unitOfWorkMock.HostProfiles.Returns(_hostProfileRepositoryMock);

            _handler = new DeleteListingCommandHandler(_unitOfWorkMock);
        }

        private static Address CreateValidAddress() =>
            Address.Create("Poland", "Warsaw", "Mazowieckie", "00-110", "ul. Marszałkowska 123");

        private static Price CreateValidPrice(decimal amount = 100) =>
            Price.Create(amount, Currency.PLN);

        [Fact]
        public async Task Handle_Should_DeleteListingAndHistory_When_NoActiveReservationsExist()
        {
            var hostId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var command = new DeleteListingCommand(listingId, hostId);

            var listing = CreateListing(hostId);
            SetId(listing, listingId);

            var hostProfile = HostProfile.Create(Guid.NewGuid());
            SetId(hostProfile, hostId);
            hostProfile.AddListingId(listingId);

            var address = CreateValidAddress();
            var price = CreateValidPrice();

            var oldReservation = Reservation.Create(listingId, Guid.NewGuid(), hostId, "T", address, price, DateTime.Now, DateTime.Now);
            SetStatus(oldReservation, ReservationStatus.Cancelled);
            var reservations = new List<Reservation> { oldReservation };

            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
                .Returns(listing);
            _hostProfileRepositoryMock.GetByIdAsync(hostId, Arg.Any<CancellationToken>())
                .Returns(hostProfile);
            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(reservations);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();

            hostProfile.ListingIds.Should().NotContain(listingId);

            _reservationRepositoryMock.Received(1).RemoveRange(reservations);
            _listingRepositoryMock.Received(1).Remove(listing);
            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnForbidden_When_UserIsNotOwner()
        {
            var ownerId = Guid.NewGuid();
            var invalidId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var listing = CreateListing(ownerId);
            SetId(listing, listingId);

            var command = new DeleteListingCommand(listingId, invalidId);

            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
                .Returns(listing);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Forbidden);
            result.FirstError.Code.Should().Be("Listing.InvalidOwner");
        }

        [Fact]
        public async Task Handle_Should_ReturnConflict_When_ActiveReservationsExist()
        {
            var hostId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var command = new DeleteListingCommand(listingId, hostId);

            var listing = CreateListing(hostId);
            SetId(listing, listingId);

            var address = CreateValidAddress();
            var price = CreateValidPrice();

            var activeReservation = Reservation.Create(listingId, Guid.NewGuid(), hostId, "T", address, price, DateTime.Now, DateTime.Now);
            SetStatus(activeReservation, ReservationStatus.InProgress);

            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>()).Returns(listing);
            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                .Returns(new List<Reservation> { activeReservation });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Conflict);
            result.FirstError.Code.Should().Be("Listing.CannotDeleteActive");

            _listingRepositoryMock.DidNotReceive().Remove(Arg.Any<Listing>());
            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_ListingDoesNotExist()
        {
            _listingRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Listing?)null);

            var result = await _handler.Handle(new DeleteListingCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Listing.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnHostProfileNotFound_When_ProfileIsMissing()
        {
            var hostId = Guid.NewGuid();
            var listing = CreateListing(hostId);

            _listingRepositoryMock.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>()).Returns(listing);
            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                 .Returns(new List<Reservation>());

            _hostProfileRepositoryMock.GetByIdAsync(hostId, Arg.Any<CancellationToken>())
                .Returns((HostProfile?)null);

            var result = await _handler.Handle(new DeleteListingCommand(listing.Id, hostId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.HostProfile.NotFound);
        }

        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_ExceptionOccurs()
        {
            var hostId = Guid.NewGuid();
            var listing = CreateListing(hostId);
            var hostProfile = HostProfile.Create(Guid.NewGuid());
            SetId(hostProfile, hostId);

            _listingRepositoryMock.GetByIdAsync(listing.Id, Arg.Any<CancellationToken>()).Returns(listing);
            _hostProfileRepositoryMock.GetByIdAsync(hostId, Arg.Any<CancellationToken>()).Returns(hostProfile);
            _reservationRepositoryMock.SearchAsync(Arg.Any<List<IFilterable<Reservation>>>(), Arg.Any<CancellationToken>())
                 .Returns(new List<Reservation>());

            _unitOfWorkMock.When(x => x.CommitAsync(Arg.Any<CancellationToken>()))
                .Do(x => { throw new Exception("Database error"); });

            var result = await _handler.Handle(new DeleteListingCommand(listing.Id, hostId), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Listing.DeleteFailed");

            await _unitOfWorkMock.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
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
            var field = typeof(Reservation).GetField($"<{nameof(Reservation.Status)}>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (field != null)
                field.SetValue(reservation, status);
            else
            {
                var prop = typeof(Reservation).GetProperty(nameof(Reservation.Status));
                prop?.SetValue(reservation, status);
            }
        }
    }
}