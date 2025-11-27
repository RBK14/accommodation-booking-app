using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Contracts.Users;
using AccommodationBooking.Domain.UserAggregate;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserResponse>()
                .Map(dest => dest.UserRole, src => src.Role);

            config.NewConfig<(UpdatePersonalDetailsRequest Request, Guid UserId), UpdatePersonalDetailsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
