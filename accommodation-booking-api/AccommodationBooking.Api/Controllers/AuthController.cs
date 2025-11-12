using AccommodationBooking.Application.Authentication.Commands.Common;
using AccommodationBooking.Application.Authentication.Commands.UpdateEmail;
using AccommodationBooking.Application.Authentication.Commands.UpdatePassword;
using AccommodationBooking.Application.Authentication.Queries.Login;
using AccommodationBooking.Contracts.Authentication;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{

    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("register-guest")]
        public async Task<IActionResult> RegisterGuest(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("register-host")]
        public async Task<IActionResult> RegisterHost(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var result = await _mediator.Send(query);

            return result.Match(
                auth => Ok(_mapper.Map<AuthResponse>(auth)),
                errors => Problem(errors));
        }

        [HttpPost("{userId:Guid}/update-email")]
        public async Task<IActionResult> UpdateEmail(UpdateEmailRequest request, Guid userId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var tokenUserId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (!isAdmin && tokenUserId != userId)
                return Forbid("Nie posiadasz uprawnień do zmiany adresu e-mail innego użytkownika.");

            var command = _mapper.Map<UpdateEmailCommand>((request, userId));
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("{userId:Guid}/update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request, Guid userId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var tokenUserId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            if (tokenUserId != userId)
                return Forbid("Nie posiadasz uprawnień do zmiany hasła innego użytkownika.");

            var command = _mapper.Map<UpdatePasswordCommand>((request, userId));
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }
    }
}
