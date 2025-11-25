using AccommodationBooking.Application.Reservations.Commands.CreateReservation;
using AccommodationBooking.Application.Reservations.Commands.UpdateReservationStatus;
using AccommodationBooking.Contracts.Reservations;
using AccommodationBooking.Domain.ReservationAggregate;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class ReservationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Reservation, ReservationResponse>()
                .Map(dest => dest.Title, src => src.ListingTitle)
                .Map(dest => dest.Country, src => src.ListingAddress.Country)
                .Map(dest => dest.City, src => src.ListingAddress.City)
                .Map(dest => dest.PostalCode, src => src.ListingAddress.PostalCode)
                .Map(dest => dest.Street, src => src.ListingAddress.Street)
                .Map(dest => dest.BuildingNumber, src => src.ListingAddress.BuildingNumber)
                .Map(dest => dest.PricePerDay, src => src.ListingPricePerDay.Amount)
                .Map(dest => dest.TotalPrice, src => src.TotalPrice.Amount)
                .Map(dest => dest.Currency, src => src.ListingPricePerDay.Currency)
                .Map(dest => dest, src => src);

            config.NewConfig<(CreateReservationRequest Request, Guid GuestProfileId), CreateReservationCommand>()
                .Map(dest => dest.GuestProfileId, src => src.GuestProfileId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(UpdateReservationStatusRequest Request, Guid ReservationId), UpdateReservationStatusCommand>()
                .Map(dest => dest.ReservationId, src => src.ReservationId)
                .Map(dest => dest, src => src.Request);

        }
    }
}
