using AccommodationBooking.Application.Authentication.Commands.RegisterAdmin;
using AccommodationBooking.Application.Authentication.Commands.RegisterGuest;
using AccommodationBooking.Application.Authentication.Commands.RegisterHost;
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
    /// <summary>
    /// Controller for authentication and user registration operations.
    /// </summary>
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Registers a new guest user.
        /// </summary>
        [HttpPost("register-guest")]
        public async Task<IActionResult> RegisterGuest(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterGuestCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Registers a new host user.
        /// </summary>
        [HttpPost("register-host")]
        public async Task<IActionResult> RegisterHost(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterHostCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Registers a new admin user.
        /// </summary>
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterAdminCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Authenticates a user and returns an access token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var result = await _mediator.Send(query);

            return result.Match(
                auth => Ok(_mapper.Map<AuthResponse>(auth)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Updates a user's email address.
        /// </summary>
        [HttpPost("{userId:guid}/update-email")]
        [Authorize]
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

        /// <summary>
        /// Updates a user's password.
        /// </summary>
        [HttpPost("{userId:guid}/update-password")]
        [Authorize]
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
