using AccommodationBooking.Domain.Common.Exceptions;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;
using FluentAssertions;

namespace AccommodationBooking.Domain.UnitTests.Aggregates.UserAggregate
{
    /// <summary>
    /// Unit tests for the User aggregate root.
    /// </summary>
    public class UserTests
    {
        [Fact]
        public void CreateGuest_Should_CreateUserWithGuestRole()
        {
            // Act
            var user = User.CreateGuest("guest@test.com", "hash123", "Jan", "Kowalski", "123456789");

            // Assert
            user.Role.Should().Be(UserRole.Guest);
            user.Email.Should().Be("guest@test.com");
        }

        [Fact]
        public void CreateHost_Should_CreateUserWithHostRole()
        {
            // Act
            var user = User.CreateHost("host@test.com", "hash123", "Anna", "Nowak", "987654321");

            // Assert
            user.Role.Should().Be(UserRole.Host);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void CreateGuest_Should_ThrowException_When_EmailIsInvalid(string invalidEmail)
        {
            // Act
            Action act = () => User.CreateGuest(invalidEmail, "hash", "Jan", "Kowalski", "123");

            // Assert
            act.Should().Throw<DomainValidationException>()
               .WithMessage("Email cannot be empty.");
        }

        [Fact]
        public void UpdateEmail_Should_UpdateEmail_When_ValueIsNew()
        {
            // Arrange
            var user = User.CreateGuest("old@test.com", "hash", "J", "K", "123");
            var originalUpdatedAt = user.UpdatedAt;

            Thread.Sleep(10);

            // Act
            user.UpdateEmail("new@test.com");

            // Assert
            user.Email.Should().Be("new@test.com");
            user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdatePersonalDetails_Should_UpdateOnlyModifiedFields()
        {
            // Arrange
            var user = User.CreateGuest("test@test.com", "hash", "OldName", "OldSurname", "OldPhone");

            // Act
            user.UpdatePersonalDetails("NewName", "OldSurname", "NewPhone");

            // Assert
            user.FirstName.Should().Be("NewName");
            user.LastName.Should().Be("OldSurname");
            user.Phone.Should().Be("NewPhone");
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdatePasswordHash_Should_ThrowException_When_HashIsEmpty()
        {
            // Arrange
            var user = User.CreateGuest("test@test.com", "validHash", "J", "K", "123");

            // Act
            Action act = () => user.UpdatePasswordHash("");

            // Assert
            act.Should().Throw<DomainValidationException>();
        }
    }
}
