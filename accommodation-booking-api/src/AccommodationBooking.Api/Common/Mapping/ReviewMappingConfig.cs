using AccommodationBooking.Application.Listings.Commands.CreateReview;
using AccommodationBooking.Application.Listings.Commands.UpdateReview;
using AccommodationBooking.Contracts.Reviews;
using AccommodationBooking.Domain.ListingAggregate.Entities;
using Mapster;

namespace AccommodationBooking.Api.Common.Mapping
{
    public class ReviewMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Review, ReviewResponse>();

            config.NewConfig<(CreateReviewRequest Request, Guid GuestProfileId), CreateReviewCommand>()
                .Map(dest => dest.GuestProfileId, src => src.GuestProfileId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(UpdateReviewRequest Request, Guid ReviewId, Guid ProfileId), UpdateReviewCommand>()
                .Map(dest => dest.ReviewId, src => src.ReviewId)
                .Map(dest => dest.ProfileId, src => src.ProfileId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
