using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AccommodationBooking.Infrastructure.Persistence;
using AccommodationBooking.Api;
using System.Linq;
using AccommodationBooking.Application.Common.Intrefaces.Authentication; // Dodaj ten namespace
using AccommodationBooking.Domain.UserAggregate; // Dodaj ten namespace

public class AccommodationBookingFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 1. Nadpisujemy konfiguracjê JWT
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"JwtSettings:Secret", "SuperTajnyKluczDoTestowKtoryMaPrzynajmniej32Znaki!"},
                {"JwtSettings:ExpiryMinutes", "60"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"}
            });
        });

        builder.ConfigureServices(services =>
        {
            // 2. TOTALNE CZYSZCZENIE
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(AppDbContext)).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // 3. Dodajemy czyst¹ bazê In-Memory
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning));
            });

            // 4. Inicjalizacja bazy
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                try
                {
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DB INIT ERROR: {ex.Message}");
                    throw;
                }
            }
        });
    }

    // --- NOWA METODA POMOCNICZA DO GENEROWANIA TOKENÓW ---
    public string GenerateAccessToken(User user, Guid profileId)
    {
        // Tworzymy scope, aby pobraæ serwis IJwtTokenGenerator z kontenera DI
        using var scope = Services.CreateScope();
        var tokenGenerator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();

        // Generator u¿yje ustawieñ z ConfigureWebHost (czyli naszego testowego klucza)
        return tokenGenerator.GenerateAccessToken(user, profileId);
    }
}