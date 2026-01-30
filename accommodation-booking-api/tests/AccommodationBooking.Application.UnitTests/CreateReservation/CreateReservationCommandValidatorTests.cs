using AccommodationBooking.Application.Reservations.Commands.CreateReservation;
using FluentValidation.TestHelper;

namespace AccommodationBooking.Application.UnitTests.CreateReservation
{
    /// <summary>
    /// Unit tests for CreateReservationCommandValidator.
    /// </summary>
    public class CreateReservationCommandValidatorTests
    {
        private readonly CreateReservationCommandValidator _validator;

        public CreateReservationCommandValidatorTests()
        {
            _validator = new CreateReservationCommandValidator();
        }

        [Fact]
        public void Validate_Should_HaveNoError_When_DatesAreCorrect()
        {
            var command = new CreateReservationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)));

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Should_Fail_When_CheckOutIsBeforeCheckIn()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var yesterday = today.AddDays(-1);

            var command = new CreateReservationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CheckIn: today,
                CheckOut: yesterday);

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.CheckOut)
                .WithErrorMessage("Data wymeldowania musi być późniejsza niż data zameldowania.");
        }

        [Fact]
        public void Validate_Should_Fail_When_CheckOutIsSameAsCheckIn()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var command = new CreateReservationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CheckIn: today,
                CheckOut: today);

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.CheckOut);
        }

        [Fact]
        public void Validate_Should_Fail_When_DatesAreEmpty()
        {
            var command = new CreateReservationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CheckIn: default,
                CheckOut: default);

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.CheckIn);

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.CheckOut);
        }
    }
}