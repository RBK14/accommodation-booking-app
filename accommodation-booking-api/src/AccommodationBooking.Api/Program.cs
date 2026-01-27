using AccommodationBooking.Application;
using AccommodationBooking.Infrastructure;

namespace AccommodationBooking.Api
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services
                    .AddPresentation()
                    .AddApplication()
                    .AddInfrastructure(builder.Configuration);

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowOrigins", policy =>
                    {
                        policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost:5174", "https://localhost:5174")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();

                    });
                });
            }


            var app = builder.Build();
            {
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseCors("AllowOrigins");

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
        }


    }
}