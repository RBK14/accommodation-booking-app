using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Contracts.Users;
using AccommodationBooking.Domain.UserAggregate.Enums;
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

        [HttpPost("{userId:Guid}/update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsRequest request, Guid userId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var tokenUserId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (!isAdmin && tokenUserId != userId)
                return Forbid("Nie posiadasz uprawnień do edycji danych innego użytkownika.");

            var command = _mapper.Map<UpdatePersonalDetailsCommand>((request, userId));
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }
    }
}