using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    [Route("api/users")]
    public class UsersController(ISender mediator) : ApiController
    {
        private readonly ISender _mediator = mediator;

        [HttpPost("update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsRequest request)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var command = new UpdatePersonalDetailsCommand(
                userId,
                request.FirstName,
                request.LastName,
                request.Phone);

            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }
    }
}