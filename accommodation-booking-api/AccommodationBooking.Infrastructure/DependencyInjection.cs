using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Infrastructure.Authentication;
using AccommodationBooking.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationBooking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services
                .AddAuth(configuration)
                .AddPersistence(configuration);

            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
        {
            // TODO: Zamienić na AddScoped po dodaniu DB
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUserRepository, UserRepository>();

            return services;
        }
    }
}
