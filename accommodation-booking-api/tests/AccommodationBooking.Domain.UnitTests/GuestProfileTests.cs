using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.GuestProfileAggregate;
using FluentAssertions;

namespace AccommodationBooking.Domain.UnitTests
{
    public class GuestProfileTests
    {
        [Fact]
        public void Create_Should_ReturnGuestProfile_When_UserIdIsValid()
        {
            var userId = Guid.NewGuid();

            var guestProfile = GuestProfile.Create(userId);

            guestProfile.Should().NotBeNull();
            guestProfile.UserId.Should().Be(userId);
            guestProfile.ReservationIds.Should().BeEmpty();
        }

        [Fact]
        public void Create_Should_ThrowException_When_UserIdIsEmpty()
        {
            Action act = () => GuestProfile.Create(Guid.Empty);

            act.Should().Throw<DomainValidationException>()
               .WithMessage("GuestProfile must be associated with a valid UserId.");
        }

        [Fact]
        public void AddReservationId_Should_AddId_When_ReservationIdIsNew()
        {
            var guestProfile = GuestProfile.Create(Guid.NewGuid());
            var reservationId = Guid.NewGuid();

            guestProfile.AddReservationId(reservationId);

            guestProfile.ReservationIds.Should().Contain(reservationId);
        }

        [Fact]
        public void AddReservationId_Should_ThrowException_When_ReservationIdIsDuplicate()
        {
            var guestProfile = GuestProfile.Create(Guid.NewGuid());
            var reservationId = Guid.NewGuid();
            guestProfile.AddReservationId(reservationId);

            Action act = () => guestProfile.AddReservationId(reservationId);

            act.Should().Throw<DomainIllegalStateException>()
               .WithMessage("The reservation is already associated with this guest.");
        }

        [Fact]
        public void RemoveReservationId_Should_RemoveId_When_ReservationIdExists()
        {
            var guestProfile = GuestProfile.Create(Guid.NewGuid());
            var reservationId = Guid.NewGuid();
            guestProfile.AddReservationId(reservationId);

            guestProfile.RemoveReservationId(reservationId);

            guestProfile.ReservationIds.Should().BeEmpty();
        }
    }
}