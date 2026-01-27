using AccommodationBooking.AcceptanceTests;
using AccommodationBooking.Contracts.Authentication; // [cite: 2066, 2067]
using AccommodationBooking.Contracts.Listings; // [cite: 2070, 2072]
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Domain.UserAggregate.Enums;
using AccommodationBooking.Infrastructure.Persistence; // [cite: 2818]
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Binding]
public class ListingSteps : IClassFixture<AccommodationBookingFactory>
{
    private readonly HttpClient _client;
    private readonly AccommodationBookingFactory _factory;
    private HttpResponseMessage _lastResponse;
    private ListingResponse _createdListing;

    public ListingSteps(AccommodationBookingFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Given(@"Jestem zarejestrowany i zalogowany jako ""(.*)""")]
    public async Task GivenJestemZarejestrowanyIZalogowanyJako(string rola)
    {
        // 1. Tworzymy obiekt użytkownika (User)
        var user = User.CreateHost(
            "Jan",
            "Kowalski",
            "host@test.com",
            "Password1!",
            "+48123456789"
        );

        // 2. Zapisujemy użytkownika w bazie danych (aby API go widziało)
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Users.Add(user);

            // Dodaj też profil (HostProfile/GuestProfile) jeśli jest wymagany przez API!
            var profileId = Guid.NewGuid();
            if (rola == "Host")
            {
                var hostProfile = HostProfile.Create(user.Id); // Dopasuj do swojej domeny
                profileId = hostProfile.Id;
                db.HostProfiles.Add(hostProfile);
            }

            await db.SaveChangesAsync();

            // 3. Generujemy token używając nowej metody w Factory
            var token = _factory.GenerateAccessToken(user, profileId);

            // 4. Ustawiamy token w kliencie HTTP
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    [When(@"Wysyłam żądanie utworzenia oferty z następującymi danymi:")]
    public async Task WhenWysylamZadanieUtworzeniaOferty(DataTable dataTable)
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

        _lastResponse = await _client.PostAsJsonAsync("/api/listings", request);
    }

    [Then(@"Odpowiedź serwera powinna mieć status 200 OK")]
    public void ThenOdpowiedzSerweraPowinnaMiecStatus200OK()
    {
        _lastResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Then(@"W odpowiedzi powinienem otrzymać szczegóły utworzonej oferty")]
    public async Task ThenWOdpowiedziPowinienemOtrzymacSzczegolyUtworzonejOferty()
    {
        _createdListing = await _lastResponse.Content.ReadFromJsonAsync<ListingResponse>();

        _createdListing.Should().NotBeNull();
        _createdListing.Title.Should().Be("Apartament Wrocław");
        _createdListing.Id.Should().NotBeEmpty();
        // Sprawdzenie czy HostProfileId zostało poprawnie przypisane z tokena
        _createdListing.HostProfileId.Should().NotBeEmpty();
    }

    [Then(@"Oferta powinna być zapisana w bazie danych")]
    public void ThenOfertaPowinnaBycZapisanaWBazieDanych()
    {
        // Tworzymy scope, aby dostać się do bazy danych wewnątrz testu
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var listingInDb = dbContext.Listings.FirstOrDefault(l => l.Id == _createdListing.Id);

        listingInDb.Should().NotBeNull();
        listingInDb.Title.Should().Be("Apartament Wrocław");
    }
}