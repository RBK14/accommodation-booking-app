namespace AccommodationBooking.FunctionalTests
{
    /// <summary>
    /// Functional tests for admin user management operations.
    /// </summary>
    public class AdminTests : BaseTest
    {
        [Fact]
        public void DeleteGuest_WithGeneratedEmail_ShouldSucceed()
        {
            var guestEmail = GenerateUniqueEmail("guest");
            RegisterNewGuest(email: guestEmail);
            Wait(2000);

            LoginAsAdmin();
            Wait(2000);

            AdminUsersPage.NavigateTo();
            Wait(2000);

            Assert.True(AdminUsersPage.IsOnAdminUsersPage(), "Should be on admin users page");
            Assert.True(AdminUsersPage.HasUsersTable(), "Users table should be visible");

            var userExistsBefore = AdminUsersPage.UserExistsByEmail(guestEmail);
            Assert.True(userExistsBefore, $"User with email {guestEmail} should exist before deletion");

            AdminUsersPage.DeleteUserByEmail(guestEmail);
            Wait(3000);

            var userExistsAfter = AdminUsersPage.UserExistsByEmail(guestEmail);
            Assert.False(userExistsAfter, $"User with email {guestEmail} should be deleted");
        }

        [Fact]
        public void DeleteHost_WithGeneratedEmail_ShouldSucceed()
        {
            var hostEmail = GenerateUniqueEmail("host");
            RegisterNewHost(email: hostEmail);
            Wait(2000);

            LoginAsAdmin();
            Wait(2000);

            AdminUsersPage.NavigateTo();
            Wait(2000);

            Assert.True(AdminUsersPage.IsOnAdminUsersPage(), "Should be on admin users page");
            Assert.True(AdminUsersPage.HasUsersTable(), "Users table should be visible");

            var userExistsBefore = AdminUsersPage.UserExistsByEmail(hostEmail);
            Assert.True(userExistsBefore, $"User with email {hostEmail} should exist before deletion");

            AdminUsersPage.DeleteUserByEmail(hostEmail);
            Wait(3000);

            var userExistsAfter = AdminUsersPage.UserExistsByEmail(hostEmail);
            Assert.False(userExistsAfter, $"User with email {hostEmail} should be deleted");
        }
    }
}
