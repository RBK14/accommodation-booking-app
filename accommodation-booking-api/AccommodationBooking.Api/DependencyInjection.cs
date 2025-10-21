using AccommodationBooking.Api.Common.Mapping;
using Microsoft.OpenApi.Models;

namespace AccommodationBooking.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddMappings();
            services.AddSwagger();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // SwaggerUI Authorization Config for Development
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
