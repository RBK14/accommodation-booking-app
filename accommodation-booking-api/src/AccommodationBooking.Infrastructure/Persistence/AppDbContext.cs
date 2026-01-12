using AccommodationBooking.Domain.GuestProfileAggregate;
using AccommodationBooking.Domain.HostProfileAggregate;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<GuestProfile> GuestProfiles { get; set; } = null!;
        public DbSet<HostProfile> HostProfiles { get; set; } = null!;
        public DbSet<Listing> Listings { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                            .SelectMany(t => t.GetProperties())
                            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                property.SetPrecision(0);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
