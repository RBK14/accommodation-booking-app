using AccommodationBooking.Contracts.Listings;
using AccommodationBooking.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AccommodationBooking.AcceptanceTests.StepDefinitions;

[Binding]
public class ListingSteps
{
    private readonly HttpClient _client;
    private readonly AccommodationBookingFactory _factory;
    private readonly ScenarioContext _scenarioContext;

    public ListingSteps(AccommodationBookingFactory factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scenarioContext = scenarioContext;
    }

    [When(@"I send a request to create a listing with the following data:")]
    public async Task WhenISendARequestToCreateAListingWithTheFollowingData(DataTable dataTable)
    {
        var row = dataTable.Rows[0];

        var request = new CreateListingRequest(
            Title: row["Title"],
            Description: row["Description"],
            AccommodationType: row["Type"],
            Beds: int.Parse(row["Beds"]),
            MaxGuests: int.Parse(row["MaxGuests"]),
            Country: row["Country"],
            City: row["City"],
            PostalCode: row["Postal"],
            Street: row["Street"],
            BuildingNumber: row["Building"],
            AmountPerDay: decimal.Parse(row["Amount"]),
            Currency: row["Currency"],
            Photos: row["Photos"].Split(',').Select(p => p.Trim()).ToList()
        );

        if (_scenarioContext.ContainsKey("AuthToken"))
        {
            var token = _scenarioContext.Get<string>("AuthToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _client.PostAsJsonAsync("/api/listings", request);
        _scenarioContext["LastResponse"] = response;
    }

    [Then(@"I should receive the details of the created listing in the response")]
    public async Task ThenIShouldReceiveTheDetailsOfTheCreatedListingInTheResponse()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>("LastResponse");
        var createdListing = await response.Content.ReadFromJsonAsync<ListingResponse>();

        createdListing.Should().NotBeNull();
        createdListing!.Title.Should().Be("Apartament Wroclaw");
        createdListing.Id.Should().NotBeEmpty();
        createdListing.HostProfileId.Should().NotBeEmpty();

        _scenarioContext["CreatedListing"] = createdListing;
    }

    [Then(@"The listing should be saved in the database")]
    public void ThenTheListingShouldBeSavedInTheDatabase()
    {
        var createdListing = _scenarioContext.Get<ListingResponse>("CreatedListing");

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var listingInDb = dbContext.Listings.FirstOrDefault(l => l.Id == createdListing.Id);

        listingInDb.Should().NotBeNull();
        listingInDb!.Title.Should().Be("Apartament Wroclaw");
    }
}
