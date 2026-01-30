using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Application.Listings.Commands.CreateListing;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.Listings.Commands.CreateListing
{
    /// <summary>
    /// Unit tests for CreateListingCommandHandler.
    /// </summary>
    public class CreateListingCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IHostProfileRepository _hostProfileRepositoryMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly CreateListingCommandHandler _handler;

        public CreateListingCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _hostProfileRepositoryMock = Substitute.For<IHostProfileRepository>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();

            _unitOfWorkMock.HostProfiles.Returns(_hostProfileRepositoryMock);
            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);

            _handler = new CreateListingCommandHandler(_unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_CreateListing_When_CommandIsValid()
        {
            var command = CreateValidCommand();

            var hostProfile = HostProfile.Create(command.HostProfileId);
            _hostProfileRepositoryMock
                .GetByIdAsync(command.HostProfileId, Arg.Any<CancellationToken>())
                .Returns(hostProfile);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Listing>();

            _listingRepositoryMock.Received(1).Add(Arg.Any<Listing>());

            await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundError_When_HostProfileDoesNotExist()
        {
            var command = CreateValidCommand();

            _hostProfileRepositoryMock
                .GetByIdAsync(command.HostProfileId, Arg.Any<CancellationToken>())
                .Returns((HostProfile?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.HostProfile.NotFound);

            _listingRepositoryMock.DidNotReceive().Add(Arg.Any<Listing>());
            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_RollbackTransaction_When_DomainValidationFails()
        {
            var invalidCommand = new CreateListingCommand(
                HostProfileId: Guid.NewGuid(),
                Title: "",
                Description: "Valid description",
                AccommodationType: "Apartment",
                Beds: 2,
                MaxGuests: 4,
                Country: "Poland",
                City: "Warsaw",
                PostalCode: "00-001",
                Street: "Test Street",
                BuildingNumber: "1",
                AmountPerDay: 100,
                Currency: "PLN",
                Photos: ["string"]
            );

            var hostProfile = HostProfile.Create(invalidCommand.HostProfileId);
            _hostProfileRepositoryMock
                .GetByIdAsync(invalidCommand.HostProfileId, Arg.Any<CancellationToken>())
                .Returns(hostProfile);

            var result = await _handler.Handle(invalidCommand, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Listing.CreationFailed);

            await _unitOfWorkMock.Received(1).RollbackAsync(Arg.Any<CancellationToken>());

            await _unitOfWorkMock.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        private static CreateListingCommand CreateValidCommand()
        {
            return new CreateListingCommand(
                HostProfileId: Guid.NewGuid(),
                Title: "Nice Apartment",
                Description: "Description",
                AccommodationType: "Apartment",
                Beds: 2,
                MaxGuests: 4,
                Country: "Poland",
                City: "Warsaw",
                PostalCode: "00-001",
                Street: "Marszalkowska",
                BuildingNumber: "10",
                AmountPerDay: 200,
                Currency: "PLN",
                Photos: ["string"]
            );
        }
    }
}
