using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    [Route("api/users")]
    public class UsersController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsRequest request)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var command = _mapper.Map<UpdatePersonalDetailsCommand>((request, userId));
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }
    }
}