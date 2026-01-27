using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace AccommodationBooking.AcceptanceTests.StepDefinitions;

[Binding]
public class CommonSteps
{
    private readonly AccommodationBookingFactory _factory;
    private readonly ScenarioContext _scenarioContext;

    public CommonSteps(AccommodationBookingFactory factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }

    [Given(@"I am registered and logged in as ""(.*)""")]
    public async Task GivenIAmRegisteredAndLoggedInAs(string role)
    {
        User user;
        Guid profileId;

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (role == "Guest")
            {
                user = User.CreateGuest(
                    "Anna",
                    "Nowak",
                    "guest@test.com",
                    "Password1!",
                    "+48987654321"
                );

                db.Users.Add(user);

                var guestProfile = GuestProfile.Create(user.Id);
                profileId = guestProfile.Id;
                db.GuestProfiles.Add(guestProfile);
            }
            else if (role == "Host")
            {
                user = User.CreateHost(
                    "Jan",
                    "Kowalski",
                    "host@test.com",
                    "Password1!",
                    "+48123456789"
                );

                db.Users.Add(user);

                var hostProfile = HostProfile.Create(user.Id);
                profileId = hostProfile.Id;
                db.HostProfiles.Add(hostProfile);
            }
            else
            {
                throw new ArgumentException($"Unknown role: {role}");
            }

            await db.SaveChangesAsync();

            var token = _factory.GenerateAccessToken(user, profileId);

            _scenarioContext["AuthToken"] = token;
            _scenarioContext["UserId"] = user.Id;
            _scenarioContext["ProfileId"] = profileId;
        }
    }

    [Then(@"The server response should have status 200 OK")]
    public async Task ThenTheServerResponseShouldHaveStatus200OK()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("LastResponse");
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Response failed with content: {errorContent}");
        }
        else
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
