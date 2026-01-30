using AccommodationBooking.Api.Common.Mapping;
using Microsoft.OpenApi.Models;

namespace AccommodationBooking.Api
{
    /// <summary>
    /// Dependency injection configuration for the Presentation layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers presentation layer services including controllers and mapping.
        /// </summary>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddMappings();
            services.AddSwagger();

            return services;
        }

        /// <summary>
        /// Configures Swagger with JWT authentication support.
        /// </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
