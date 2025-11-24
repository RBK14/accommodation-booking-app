using AccommodationBooking.Application.Listings.Commands.CreateListing;
using AccommodationBooking.Application.Listings.Commands.UpdateListing;
using AccommodationBooking.Application.Listings.Queries.GetAvailableDates;
using AccommodationBooking.Application.Listings.Queries.GetListing;
using AccommodationBooking.Application.Listings.Queries.GetListings;
using AccommodationBooking.Contracts.Listings;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    [Route("api/listings")]
    public class ListingController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        [Authorize(Roles = "Host")]
        public async Task<IActionResult> CreateListing(CreateListingRequest request)
        {
            var profileIdValue = User.FindFirstValue("ProfileId");
            if (!Guid.TryParse(profileIdValue, out var profileId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var command = _mapper.Map<CreateListingCommand>((request, profileId));
            var result = await _mediator.Send(command);

            return result.Match(
                listing => Ok(_mapper.Map<ListingResponse>(listing)),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetListings(Guid? hostProfileId)
        {
            var query = new GetListingsQuery(hostProfileId);
            var result = await _mediator.Send(query);

            var response = result
                .Select(listing => _mapper.Map<ListingResponse>(listing))
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetListing(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator oferty jest nieprawidłowy.");

            var query = new GetListingQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                listing => Ok(_mapper.Map<ListingResponse>(listing)),
                errors => Problem(errors));
        }

        [HttpPost("{id:guid}")]
        [Authorize(Roles = "Admin, Host")]
        public async Task<IActionResult> UpdateListing(UpdateListingRequest request, Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator oferty jest nieprawidłowy.");

            var profileId = Guid.Empty;

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            if (roleValue.IsInRole(UserRole.Host))
            {
                var profileIdValue = User.FindFirstValue("ProfileId");
                if (Guid.TryParse(profileIdValue, out var tokenProfileId))
                    profileId = tokenProfileId;
            }

            var command = _mapper.Map<UpdateListingCommand>((request, id, profileId));
            var result = await _mediator.Send(command);

            return result.Match(
                listing => Ok(_mapper.Map<ListingResponse>(listing)),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/get-dates")]
        public async Task<IActionResult> GetAvailableDates(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator oferty jest nieprawidłowy.");

            var query = new GetAvailableDatesQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                dates => Ok(dates.ToList()),
                errors => Problem(errors));
        }
    }
}
