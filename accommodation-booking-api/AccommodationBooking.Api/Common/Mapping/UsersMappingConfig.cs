using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Contracts.Users;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class UsersMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(UpdatePersonalDetailsRequest Request, Guid UserId), UpdatePersonalDetailsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
