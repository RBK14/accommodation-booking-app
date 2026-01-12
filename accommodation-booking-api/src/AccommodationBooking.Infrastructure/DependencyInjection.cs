using AccommodationBooking.Application.Common.Intrefaces.Authentication;
using AccommodationBooking.Application.Common.Intrefaces.Persistence;
using AccommodationBooking.Infrastructure.Authentication;
using AccommodationBooking.Infrastructure.BackgroundJobs;
using AccommodationBooking.Infrastructure.Persistence;
using AccommodationBooking.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGuestProfileRepository, GuestProfileRepository>();
            services.AddScoped<IHostProfileRepository, HostProfileRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            services.AddHostedService<ReservationStatusUpdaterService>();

            return services;
        }
    }
}
