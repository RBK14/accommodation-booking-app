using AccommodationBooking.Application.Listings.Commands.CreateReview;
using AccommodationBooking.Application.Listings.Commands.DeleteReview;
using AccommodationBooking.Application.Listings.Commands.UpdateReview;
using AccommodationBooking.Application.Listings.Queries.GetReview;
using AccommodationBooking.Application.Listings.Queries.GetReviews;
using AccommodationBooking.Contracts.Reviews;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    [Route("api/reviews")]
    public class ReviewsController(ISender mediator, IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost()]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> CreateReview(CreateReviewRequest request)
        {
            var profileIdValue = User.FindFirstValue("ProfileId");
            if (!Guid.TryParse(profileIdValue, out var guestProfileId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var command = _mapper.Map<CreateReviewCommand>((request, guestProfileId));
            var result = await _mediator.Send(command);

            return result.Match(
                review => Ok(_mapper.Map<ReviewResponse>(review)),
                errors => Problem(errors));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Guest")]
        public async Task<IActionResult> UpdateReview(UpdateReviewRequest request, Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator opinii jest nieprawidłowy.");

            var profileId = Guid.Empty;

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            if (roleValue.IsInRole(UserRole.Guest))
            {
                var profileIdValue = User.FindFirstValue("ProfileId");
                if (Guid.TryParse(profileIdValue, out var tokenProfileId))
                    profileId = tokenProfileId;
            }

            var command = _mapper.Map<UpdateReviewCommand>((request, id, profileId));
            var result = await _mediator.Send(command);

            return result.Match(
                review => Ok(_mapper.Map<ReviewResponse>(review)),
                errors => Problem(errors));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Guest")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator opinii jest nieprawidłowy.");

            var profileId = Guid.Empty;

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            if (roleValue.IsInRole(UserRole.Guest))
            {
                var profileIdValue = User.FindFirstValue("ProfileId");
                if (Guid.TryParse(profileIdValue, out var tokenProfileId))
                    profileId = tokenProfileId;
            }

            var command = new DeleteReviewCommand(id, profileId);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetListingReviews(Guid? listingId, Guid? guestProfileId)
        {
            var query = new GetReviewsQuery(listingId, guestProfileId);
            var result = await _mediator.Send(query);

            var response = result
                .Select(review => _mapper.Map<ReviewResponse>(review))
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator opinii jest nieprawidłowy.");

            var query = new GetReviewQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                review => Ok(_mapper.Map<ReviewResponse>(review)),
                errors => Problem(errors));
        }
    }
}