using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Application.Listings.Queries.GetAvailableDates;
using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Errors;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using FluentAssertions;
using NSubstitute;

namespace AccommodationBooking.Application.UnitTests.GetAvailableDates
{
    /// <summary>
    /// Unit tests for GetAvailableDatesQueryHandler.
    /// </summary>
    public class GetAvailableDatesQueryHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IListingRepository _listingRepositoryMock;
        private readonly GetAvailableDatesQueryHandler _handler;

        public GetAvailableDatesQueryHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _listingRepositoryMock = Substitute.For<IListingRepository>();
            _unitOfWorkMock.Listings.Returns(_listingRepositoryMock);

            _handler = new GetAvailableDatesQueryHandler(_unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnAvailableDates_When_ListingHasNoReservations()
        {
            var listingId = Guid.NewGuid();
            var listing = CreateListing();

            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
                .Returns(listing);

            var startSearch = DateOnly.FromDateTime(DateTime.UtcNow);
            var query = new GetAvailableDatesQuery(listingId, startSearch, 3);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Should().HaveCount(4);
        }

        [Fact]
        public async Task Handle_Should_ExcludeOccupiedDates_But_IncludeCheckOutDate()
        {
            var listingId = Guid.NewGuid();
            var listing = CreateListing();

            var futureYear = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)).Year;

            var searchStart = new DateOnly(futureYear, 1, 1);
            var days = 9;
            var resStart = searchStart.AddDays(3).ToDateTime(new TimeOnly(14, 0));
            var resEnd = searchStart.AddDays(5).ToDateTime(new TimeOnly(10, 0));

            listing.ReserveDates(Guid.NewGuid(), resStart, resEnd);

            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
                .Returns(listing);

            var query = new GetAvailableDatesQuery(listingId, searchStart, days);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsError.Should().BeFalse();
            var availableDates = result.Value.ToList();


            availableDates.Should().Contain(new DateOnly(futureYear, 1, 1));
            availableDates.Should().Contain(new DateOnly(futureYear, 1, 2));
            availableDates.Should().Contain(new DateOnly(futureYear, 1, 3));

            availableDates.Should().NotContain(new DateOnly(futureYear, 1, 4));
            availableDates.Should().NotContain(new DateOnly(futureYear, 1, 5));

            availableDates.Should().Contain(new DateOnly(futureYear, 1, 6));
            availableDates.Should().Contain(new DateOnly(futureYear, 1, 7));
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundError_When_ListingDoesNotExist()
        {
            var query = new GetAvailableDatesQuery(Guid.NewGuid(), null, null);

            _listingRepositoryMock.GetByIdAsync(query.ListingId, Arg.Any<CancellationToken>())
                .Returns((Listing?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Listing.NotFound);
        }

        [Fact]
        public async Task Handle_Should_HandleDefaultValues_When_QueryParametersAreNull()
        {
            var listingId = Guid.NewGuid();
            var listing = CreateListing();
            _listingRepositoryMock.GetByIdAsync(listingId, Arg.Any<CancellationToken>())
                .Returns(listing);

            var query = new GetAvailableDatesQuery(listingId, null, null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Should().HaveCount(15);
        }

        private static Listing CreateListing()
        {
            return Listing.Create(
                Guid.NewGuid(),
                "Title", "Desc", AccommodationType.Apartment, 2, 4,
                "PL", "City", "00-000", "Str", "1",
                100, Currency.PLN);
        }
    }
}