namespace AccommodationBooking.FunctionalTests
{
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

            Assert.True(AdminUsersPage.IsOnAdminUsersPage(), "Powinna być na stronie administratora - użytkownicy");
            Assert.True(AdminUsersPage.HasUsersTable(), "Powinna być widoczna tabela użytkowników");

            var userExistsBefore = AdminUsersPage.UserExistsByEmail(guestEmail);
            Assert.True(userExistsBefore, $"Użytkownik o emailu {guestEmail} powinien istnieć przed usunięciem");

            AdminUsersPage.DeleteUserByEmail(guestEmail);
            Wait(3000);

            var userExistsAfter = AdminUsersPage.UserExistsByEmail(guestEmail);
            Assert.False(userExistsAfter, $"U?ytkownik o emailu {guestEmail} powinien by? usuni?ty");
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

            Assert.True(AdminUsersPage.IsOnAdminUsersPage(), "Powinna by? na stronie administratora - u?ytkownicy");
            Assert.True(AdminUsersPage.HasUsersTable(), "Powinna by? widoczna tabela u?ytkowników");

            var userExistsBefore = AdminUsersPage.UserExistsByEmail(hostEmail);
            Assert.True(userExistsBefore, $"U?ytkownik o emailu {hostEmail} powinien istnie? przed usuni?ciem");

            AdminUsersPage.DeleteUserByEmail(hostEmail);
            Wait(3000);

            var userExistsAfter = AdminUsersPage.UserExistsByEmail(hostEmail);
            Assert.False(userExistsAfter, $"U?ytkownik o emailu {hostEmail} powinien by? usuni?ty");
        }
    }
}
