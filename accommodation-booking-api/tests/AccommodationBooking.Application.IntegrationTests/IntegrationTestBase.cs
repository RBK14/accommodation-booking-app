using AccommodationBooking.Application.Common.Interfaces.Persistence;
using AccommodationBooking.Infrastructure.Persistence;
using AccommodationBooking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccommodationBooking.Application.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        static protected AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        static protected IUnitOfWork CreateUnitOfWork(AppDbContext context)
        {
            var users = new UserRepository(context);
            var guestProfiles = new GuestProfileRepository(context);
            var hostProfiles = new HostProfileRepository(context);
            var listings = new ListingRepository(context);
            var reservations = new ReservationRepository(context);

            return new TestUnitOfWork(
                context,
                users,
                guestProfiles,
                hostProfiles,
                listings,
                reservations
            );
        }
    }

    public class TestUnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IGuestProfileRepository guestProfiles,
        IHostProfileRepository hostProfiles,
        IListingRepository listings,
        IReservationRepository reservations) : UnitOfWork(context, users, guestProfiles, hostProfiles, listings, reservations)
    {
        public override Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
