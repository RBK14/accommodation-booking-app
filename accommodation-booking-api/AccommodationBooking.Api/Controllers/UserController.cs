using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Application.Users.Queries.GetUser;
using AccommodationBooking.Application.Users.Queries.GetUsers;
using AccommodationBooking.Contracts.Users;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    [Route("api/users")]
    public class UserController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("{id:guid}/update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsRequest request, Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var tokenUserId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (!isAdmin && tokenUserId != id)
                return Forbid("Nie posiadasz uprawnień do edycji danych innego użytkownika.");

            var query = _mapper.Map<UpdatePersonalDetailsCommand>((request, id));
            var result = await _mediator.Send(query);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        // TODO: DeleteGuest, DeleteHost, DeleteAdmin

        [HttpGet]
        public async Task<IActionResult> GetUsers(string? userRole)
        {
            var query = new GetUsersQuery(userRole);
            var result = await _mediator.Send(query);

            var response = result
                .Select(user => _mapper.Map<UserResponse>(user))
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator użytownika jest nieprawidłowy.");

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (id != userId && !isAdmin)
                return Forbid("Nie posiadasz uprawnień do przeglądania profilu innego użytkownika.");

            var query = new GetUserQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                user => Ok(_mapper.Map<UserResponse>(user)),
                errors => Problem(errors));
        }
    }
}