using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.Common.ValueObjects;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using FluentAssertions;

namespace AccommodationBooking.Domain.UnitTests
{
    /// <summary>
    /// Unit tests for the Reservation aggregate root.
    /// </summary>
    public class ReservationTests
    {
        private static Address CreateValidAddress() =>
            Address.Create("Poland", "Warsaw", "Mazowieckie", "00-110", "ul. Marszalkowska 123");

        private static Price CreateValidPrice(decimal amount = 100) =>
            Price.Create(amount, Currency.PLN);

        [Fact]
        public void Create_Should_ReturnReservation_When_DataIsValid()
        {
            var listingId = Guid.NewGuid();
            var guestId = Guid.NewGuid();
            var hostId = Guid.NewGuid();

            var checkIn = DateTime.UtcNow.Date;
            var checkOut = DateTime.UtcNow.Date.AddHours(46);

            var pricePerDay = CreateValidPrice(100);

            var reservation = Reservation.Create(
                listingId,
                guestId,
                hostId,
                "Cozy apartment in the Center",
                CreateValidAddress(),
                pricePerDay,
                checkIn,
                checkOut);

            reservation.Should().NotBeNull();
            reservation.Id.Should().NotBeEmpty();
            reservation.Status.Should().Be(ReservationStatus.Accepted);

            reservation.TotalPrice.Amount.Should().Be(200);
            reservation.TotalPrice.Currency.Should().Be(Currency.PLN);
        }

        [Fact]
        public void Create_Should_ThrowException_When_CheckInDateIsAfterCheckOut()
        {
            var checkIn = DateTime.UtcNow.AddDays(5);
            var checkOut = DateTime.UtcNow.AddDays(1);

            Action act = () => Reservation.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Listing title",
                CreateValidAddress(),
                CreateValidPrice(),
                checkIn,
                checkOut);

            act.Should().Throw<DomainValidationException>()
               .WithMessage("*Check-out date must be later than check-in date*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void Create_Should_ThrowException_When_TitleIsInvalid(string invalidTitle)
        {
            Action act = () => Reservation.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                invalidTitle,
                CreateValidAddress(),
                CreateValidPrice(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1));

            act.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void MarkAsInProgress_Should_ChangeStatus_When_StatusIsAccepted()
        {
            var reservation = CreateReservationInStatus(ReservationStatus.Accepted);

            reservation.MarkAsInProgress();

            reservation.Status.Should().Be(ReservationStatus.InProgress);
            reservation.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void MarkAsInProgress_Should_ThrowException_When_StatusIsNotAccepted()
        {
            var reservation = CreateReservationInStatus(ReservationStatus.Completed);

            Action act = () => reservation.MarkAsInProgress();

            act.Should().Throw<DomainIllegalStateException>()
               .WithMessage("Reservation must be accepted to start.");
        }

        [Fact]
        public void Cancel_Should_ChangeStatusToCancelled_When_ReservationIsInProgress()
        {
            var reservation = CreateReservationInStatus(ReservationStatus.InProgress);

            reservation.Cancel();

            reservation.Status.Should().Be(ReservationStatus.Cancelled);
        }

        [Fact]
        public void Cancel_Should_ThrowException_When_ReservationIsAlreadyCompleted()
        {
            var reservation = CreateReservationInStatus(ReservationStatus.Completed);

            Action act = () => reservation.Cancel();

            act.Should().Throw<DomainIllegalStateException>()
               .WithMessage("Cannot cancel a completed reservation.");
        }

        private static Reservation CreateReservationInStatus(ReservationStatus targetStatus)
        {
            var reservation = Reservation.Create(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Listing title", CreateValidAddress(), CreateValidPrice(),
                DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            if (targetStatus == ReservationStatus.Accepted)
                return reservation;

            if (targetStatus == ReservationStatus.InProgress)
            {
                reservation.MarkAsInProgress();
                return reservation;
            }

            if (targetStatus == ReservationStatus.Completed)
            {
                reservation.MarkAsInProgress();
                reservation.MarkAsCompleted();
                return reservation;
            }

            if (targetStatus == ReservationStatus.Cancelled)
            {
                reservation.Cancel();
                return reservation;
            }

            return reservation;
        }
    }
}