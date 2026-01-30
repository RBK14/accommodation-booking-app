using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AccommodationBooking.Infrastructure.Persistence;
using AccommodationBooking.Api;
using AccommodationBooking.Application.Common.Interfaces.Authentication;
using AccommodationBooking.Domain.UserAggregate;
using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Infrastructure.Persistence.Repositories;
using AccommodationBooking.Infrastructure.BackgroundJobs;

namespace AccommodationBooking.AcceptanceTests
{
    public class AccommodationBookingFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
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
                // Remove the background service
                var hostedServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHostedService) && d.ImplementationType == typeof(ReservationStatusUpdaterService));
                if (hostedServiceDescriptor != null)
                {
                    services.Remove(hostedServiceDescriptor);
                }

                // Remove all DbContext and EF Core related services
                var descriptorsToRemove = services
                    .Where(d => d.ServiceType.FullName != null &&
                        (d.ServiceType.FullName.Contains("EntityFramework") ||
                         d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                         d.ServiceType == typeof(DbContextOptions) ||
                         d.ServiceType == typeof(AppDbContext)))
                    .ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Remove repository registrations
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IUnitOfWork)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IUserRepository)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IGuestProfileRepository)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IHostProfileRepository)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IListingRepository)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(IReservationRepository)));

                // Add InMemory DbContext
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning));
                });

                // Re-register repositories with InMemory DbContext
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IGuestProfileRepository, GuestProfileRepository>();
                services.AddScoped<IHostProfileRepository, HostProfileRepository>();
                services.AddScoped<IListingRepository, ListingRepository>();
                services.AddScoped<IReservationRepository, ReservationRepository>();
                services.AddScoped<IUnitOfWork, TestUnitOfWork>();

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

        public string GenerateAccessToken(User user, Guid profileId)
        {
            using var scope = Services.CreateScope();
            var tokenGenerator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
            return tokenGenerator.GenerateAccessToken(user, profileId);
        }
    }

    public class TestUnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IGuestProfileRepository guestProfiles,
        IHostProfileRepository hostProfiles,
        IListingRepository listings,
        IReservationRepository reservations)
        : UnitOfWork(context, users, guestProfiles, hostProfiles, listings, reservations)
    {
        public override Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
