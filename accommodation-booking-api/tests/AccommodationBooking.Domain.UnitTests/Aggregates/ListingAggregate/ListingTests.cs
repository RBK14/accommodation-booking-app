using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using FluentAssertions;

namespace AccommodationBooking.Domain.UnitTests.Aggregates.ListingAggregate
{
    /// <summary>
    /// Unit tests for the Listing aggregate root.
    /// </summary>
    public class ListingTests
    {
        private static Listing CreateValidListing()
        {
            return Listing.Create(
                Guid.NewGuid(),
                "Luxury apartment",
                "Apartment description...",
                AccommodationType.Apartment,
                2,
                4,
                "Poland", "Krakow", "30-001", "Main Square", "1",
                250,
                Currency.PLN);
        }

        [Fact]
        public void Create_Should_ReturnListing_When_DataIsValid()
        {
            var hostId = Guid.NewGuid();
            var title = "Cozy mountain cottage";

            var listing = Listing.Create(
                hostId,
                title,
                "Beautiful view of the Tatras",
                AccommodationType.House,
                4,
                6,
                "Poland", "Zakopane", "34-500", "Krupowki", "10",
                500,
                Currency.PLN);

            listing.Should().NotBeNull();
            listing.Id.Should().NotBeEmpty();
            listing.Title.Should().Be(title);
            listing.Beds.Should().Be(4);
            listing.PricePerDay.Amount.Should().Be(500);
            listing.ScheduleSlots.Should().BeEmpty();
            listing.Reviews.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_Should_ThrowException_When_BedsCountIsInvalid(int invalidBeds)
        {
            Action act = () => Listing.Create(
                Guid.NewGuid(), "Title", "Desc", AccommodationType.Apartment,
                invalidBeds, 4,
                "PL", "City", "00-000", "Str", "1", 100, Currency.PLN);

            act.Should().Throw<DomainValidationException>()
               .WithMessage("Listing must have at least one bed.");
        }

        [Fact]
        public void ReserveDates_Should_AddScheduleSlot_When_DatesAreFree()
        {
            var listing = CreateValidListing();
            var reservationId = Guid.NewGuid();

            var start = DateTime.UtcNow.AddDays(10);
            var end = DateTime.UtcNow.AddDays(15);

            listing.ReserveDates(reservationId, start, end);

            listing.ReservationIds.Should().Contain(reservationId);
            listing.ScheduleSlots.Should().HaveCount(1);
            listing.ScheduleSlots.First().ReservationId.Should().Be(reservationId);
            listing.ScheduleSlots.First().StartDate.Should().Be(start);
        }

        [Fact]
        public void ReserveDates_Should_ThrowException_When_DatesOverlap()
        {
            var listing = CreateValidListing();

            var existingStart = DateTime.UtcNow.AddDays(10);
            var existingEnd = DateTime.UtcNow.AddDays(20);

            listing.ReserveDates(Guid.NewGuid(), existingStart, existingEnd);

            var newStart = DateTime.UtcNow.AddDays(12);
            var newEnd = DateTime.UtcNow.AddDays(15);

            Action act = () => listing.ReserveDates(Guid.NewGuid(), newStart, newEnd);

            act.Should().Throw<DomainValidationException>()
               .WithMessage("The specified time range is already reserved.");
        }

        [Fact]
        public void AddReview_Should_AddReview_When_GuestHasNotReviewedYet()
        {
            var listing = CreateValidListing();
            var guestId = Guid.NewGuid();
            var rating = 5;
            var comment = "Wonderful place!";

            listing.AddReview(guestId, rating, comment);

            listing.Reviews.Should().HaveCount(1);
            var addedReview = listing.Reviews.First();
            addedReview.GuestProfileId.Should().Be(guestId);
            addedReview.Rating.Should().Be(5);
            addedReview.Comment.Should().Be(comment);
        }

        [Fact]
        public void AddReview_Should_ThrowException_When_GuestAlreadyReviewed()
        {
            var listing = CreateValidListing();
            var guestId = Guid.NewGuid();

            listing.AddReview(guestId, 5, "First review");

            Action act = () => listing.AddReview(guestId, 3, "Second review");

            act.Should().Throw<DomainIllegalStateException>()
               .WithMessage("Guest has already reviewed this listing.");
        }

        [Fact]
        public void UpdateListing_Should_UpdateFields_When_DataIsValid()
        {
            var listing = CreateValidListing();
            var newTitle = "Changed title";
            var newDesc = "New description";
            var newPrice = Price.Create(999, Currency.PLN);
            var originalUpdatedAt = listing.UpdatedAt;

            Thread.Sleep(10);

            listing.UpdateListing(
                newTitle,
                newDesc,
                listing.AccommodationType,
                listing.Beds,
                listing.MaxGuests,
                listing.Address,
                newPrice);

            listing.Title.Should().Be(newTitle);
            listing.Description.Should().Be(newDesc);
            listing.PricePerDay.Should().Be(newPrice);
            listing.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void CancelReservation_Should_RemoveSlot_When_ReservationExists()
        {
            var listing = CreateValidListing();
            var reservationId = Guid.NewGuid();
            listing.ReserveDates(reservationId, DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(6));

            listing.CancelReservation(reservationId);

            listing.ReservationIds.Should().NotContain(reservationId);
            listing.ScheduleSlots.Should().BeEmpty();
        }
    }
}
