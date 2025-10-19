using AccommodationBooking.Application.Authentication.Commands.RegisterGuest;
using AccommodationBooking.Application.Authentication.Commands.RegisterHost;
using AccommodationBooking.Application.Authentication.Queries.Login;
using AccommodationBooking.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationBooking.Api.Controllers
{

    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController(ISender mediator) : ApiController
    {
        private readonly ISender _mediator = mediator;

        [HttpPost("register-guest")]
        public async Task<IActionResult> RegisterGuest(RegisterRequest request)
        {
            var command = new RegisterGuestCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Phone);

            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("register-host")]
        public async Task<IActionResult> RegisterHost(RegisterRequest request)
        {
            var command = new RegisterHostCommand(
               request.Email,
               request.Password,
               request.FirstName,
               request.LastName,
               request.Phone);

            var result = await _mediator.Send(command);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(
                request.Email,
                request.Password);

            var result = await _mediator.Send(query);

            return result.Match(
                auth => Ok(new AuthResponse(auth.User.Id.ToString(), auth.AccessToken)),
                errors => Problem(errors));
        }
    }
}
