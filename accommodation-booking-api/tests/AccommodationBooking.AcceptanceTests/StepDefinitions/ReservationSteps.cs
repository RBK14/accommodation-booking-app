using AccommodationBooking.Contracts.Reservations;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AccommodationBooking.AcceptanceTests.StepDefinitions;

[Binding]
public class ReservationSteps
{
    private readonly HttpClient _client;
    private readonly AccommodationBookingFactory _factory;
    private readonly ScenarioContext _scenarioContext;

    public ReservationSteps(AccommodationBookingFactory factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scenarioContext = scenarioContext;
    }

    [Given(@"A listing exists with the following data:")]
    public async Task GivenAListingExistsWithTheFollowingData(DataTable dataTable)
    {
        var row = dataTable.Rows[0];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Create a host for the listing
            var host = User.CreateHost(
                "Listing",
                "Owner",
                "listingowner@test.com",
                "Password1!",
                "+48111222333"
            );
            db.Users.Add(host);

            var hostProfile = HostProfile.Create(host.Id);
            db.HostProfiles.Add(hostProfile);

            await db.SaveChangesAsync();

            // Create listing using domain model
            var listing = Listing.Create(
                hostProfile.Id,
                row["Title"],
                row["Description"],
                Domain.ListingAggregate.Enums.AccommodationTypeExtensions.Parse(row["Type"]),
                int.Parse(row["Beds"]),
                int.Parse(row["MaxGuests"]),
                row["Country"],
                row["City"],
                row["Postal"],
                row["Street"],
                row["Building"],
                decimal.Parse(row["Amount"]),
                Domain.Common.Enums.CurrencyExtensions.Parse(row["Currency"])
            );

            listing.UpdatePhotos(row["Photos"].Split(',').Select(p => p.Trim()));

            db.Listings.Add(listing);
            await db.SaveChangesAsync();

            _scenarioContext["ListingId"] = listing.Id;
            _scenarioContext["HostProfileId"] = hostProfile.Id;
        }
    }

    [Given(@"The listing has an existing reservation from (.*) days from now to (.*) days from now")]
    public async Task GivenTheListingHasAnExistingReservationFromDaysFromNowToDaysFromNow(int checkInDays, int checkOutDays)
    {
        var listingId = _scenarioContext.Get<Guid>("ListingId");

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Create another guest for the existing reservation
            var existingGuest = User.CreateGuest(
                "Existing",
                "Guest",
                "existingguest@test.com",
                "Password1!",
                "+48999888777"
            );
            db.Users.Add(existingGuest);

            var existingGuestProfile = Domain.GuestProfileAggregate.GuestProfile.Create(existingGuest.Id);
            db.GuestProfiles.Add(existingGuestProfile);

            await db.SaveChangesAsync();

            // Get the listing and create a reservation
            var listing = await db.Listings.FindAsync(listingId);
            if (listing == null)
                throw new InvalidOperationException("Listing not found");

            var checkIn = DateTime.UtcNow.AddDays(checkInDays);
            var checkOut = DateTime.UtcNow.AddDays(checkOutDays);

            listing.ReserveDates(Guid.NewGuid(), checkIn, checkOut);

            await db.SaveChangesAsync();

            _scenarioContext["ExistingReservationCheckIn"] = DateOnly.FromDateTime(checkIn);
            _scenarioContext["ExistingReservationCheckOut"] = DateOnly.FromDateTime(checkOut);
        }
    }

    [When(@"I send a request to create a reservation with the following data:")]
    public async Task WhenISendARequestToCreateAReservationWithTheFollowingData(DataTable dataTable)
    {
        var row = dataTable.Rows[0];
        var listingId = _scenarioContext.Get<Guid>("ListingId");

        // Calculate dates relative to today
        var checkInDaysFromNow = int.Parse(row["CheckInDaysFromNow"]);
        var checkOutDaysFromNow = int.Parse(row["CheckOutDaysFromNow"]);

        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(checkInDaysFromNow));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(checkOutDaysFromNow));

        var request = new CreateReservationRequest(
            ListingId: listingId,
            CheckIn: checkIn,
            CheckOut: checkOut
        );

        // Set authorization header from scenario context
        if (_scenarioContext.ContainsKey("AuthToken"))
        {
            var token = _scenarioContext.Get<string>("AuthToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.PostAsJsonAsync("/api/reservations", request);
        _scenarioContext["LastResponse"] = response;
    }

    [When(@"I request available dates for the listing starting from (.*) days from now for (.*) days")]
    public async Task WhenIRequestAvailableDatesForTheListingStartingFromDaysFromNowForDays(int startDaysFromNow, int numberOfDays)
    {
        var listingId = _scenarioContext.Get<Guid>("ListingId");
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(startDaysFromNow));

        // Set authorization header from scenario context
        if (_scenarioContext.ContainsKey("AuthToken"))
        {
            var token = _scenarioContext.Get<string>("AuthToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.GetAsync($"/api/listings/{listingId}/get-dates?from={startDate:yyyy-MM-dd}&days={numberOfDays}");
        _scenarioContext["LastResponse"] = response;
    }

    [Then(@"I should receive the details of the created reservation in the response")]
    public async Task ThenIShouldReceiveTheDetailsOfTheCreatedReservationInTheResponse()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("LastResponse");
        var createdReservation = await response.Content.ReadFromJsonAsync<ReservationResponse>();

        createdReservation.Should().NotBeNull();
        createdReservation!.Id.Should().NotBeEmpty();
        createdReservation.ListingId.Should().Be(_scenarioContext.Get<Guid>("ListingId"));
        createdReservation.GuestProfileId.Should().Be(_scenarioContext.Get<Guid>("ProfileId"));
        createdReservation.Status.Should().Be("Accepted");
        createdReservation.Title.Should().NotBeNullOrEmpty();

        _scenarioContext["CreatedReservation"] = createdReservation;
    }

    [Then(@"I should receive (.*) available dates in the response")]
    public async Task ThenIShouldReceiveAvailableDatesInTheResponse(int expectedCount)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("LastResponse");
        var availableDates = await response.Content.ReadFromJsonAsync<List<DateOnly>>();

        availableDates.Should().NotBeNull();
        availableDates!.Count.Should().Be(expectedCount);

        _scenarioContext["AvailableDates"] = availableDates;
    }

    [Then(@"The available dates should not include the occupied period")]
    public async Task ThenTheAvailableDatesShouldNotIncludeTheOccupiedPeriod()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("LastResponse");
        var availableDates = await response.Content.ReadFromJsonAsync<List<DateOnly>>();

        availableDates.Should().NotBeNull();

        var checkIn = _scenarioContext.Get<DateOnly>("ExistingReservationCheckIn");
        var checkOut = _scenarioContext.Get<DateOnly>("ExistingReservationCheckOut");

        // Check that no occupied dates are in the available list
        for (var date = checkIn; date < checkOut; date = date.AddDays(1))
        {
            availableDates.Should().NotContain(date, 
                $"because {date:yyyy-MM-dd} is occupied by an existing reservation");
        }

        // The check-out date should be available (guest checks out in the morning)
        availableDates.Should().Contain(checkOut,
            "because the check-out date should be available for new check-ins");

        _scenarioContext["AvailableDates"] = availableDates;
    }

    [Then(@"The reservation should be saved in the database")]
    public void ThenTheReservationShouldBeSavedInTheDatabase()
    {
        var createdReservation = _scenarioContext.Get<ReservationResponse>("CreatedReservation");

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var reservationInDb = dbContext.Reservations.FirstOrDefault(r => r.Id == createdReservation.Id);

        reservationInDb.Should().NotBeNull();
        reservationInDb!.ListingId.Should().Be(createdReservation.ListingId);
        reservationInDb.GuestProfileId.Should().Be(createdReservation.GuestProfileId);
    }
}
