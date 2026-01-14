using AccommodationBooking.Application.Listings.Commands.CreateListing;
using AccommodationBooking.Application.Listings.Commands.UpdateListing;
using AccommodationBooking.Contracts.Listings;
using AccommodationBooking.Domain.ListingAggregate;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class ListingMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Listing, ListingResponse>()
                .Map(dest => dest.Country, src => src.Address.Country)
                .Map(dest => dest.City, src => src.Address.City)
                .Map(dest => dest.PostalCode, src => src.Address.PostalCode)
                .Map(dest => dest.Street, src => src.Address.Street)
                .Map(dest => dest.BuildingNumber, src => src.Address.BuildingNumber)
                .Map(dest => dest.AmountPerDay, src => src.PricePerDay.Amount)
                .Map(dest => dest.Currency, src => src.PricePerDay.Currency)
                .Map(dest => dest.Photos, src => src.PhotoUrls)
                .Map(dest => dest, src => src);

            config.NewConfig<(CreateListingRequest Request, Guid HostProfileId), CreateListingCommand>()
                .Map(dest => dest.HostProfileId, src => src.HostProfileId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(UpdateListingRequest Request, Guid ListingId, Guid ProfileId), UpdateListingCommand>()
                .Map(dest => dest.ListingId, src => src.ListingId)
                .Map(dest => dest.ProfileId, src => src.ProfileId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
