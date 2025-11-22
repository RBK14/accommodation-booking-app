using AccommodationBooking.Application.Authentication.Commands.Common;
using AccommodationBooking.Application.Authentication.Commands.UpdateEmail;
using AccommodationBooking.Application.Authentication.Commands.UpdatePassword;
using AccommodationBooking.Application.Authentication.Common;
using AccommodationBooking.Application.Authentication.Queries.Login;
using AccommodationBooking.Contracts.Authentication;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class AuthMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterUserCommand>();

            config.NewConfig<LoginRequest, LoginQuery>();

            config.NewConfig<AuthResultDto, AuthResponse>()
                .Map(dest => dest, src => src.User)
                .Map(dest => dest.AccessToken, src => src.AccessToken);

            config.NewConfig<(UpdateEmailRequest Request, Guid UserId), UpdateEmailCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(UpdatePasswordRequest Request, Guid UserId), UpdatePasswordCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
