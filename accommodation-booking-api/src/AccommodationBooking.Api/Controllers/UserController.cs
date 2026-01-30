using AccommodationBooking.Application.Users.Commands.DeleteAdmin;
using AccommodationBooking.Application.Users.Commands.DeleteGuest;
using AccommodationBooking.Application.Users.Commands.DeleteHost;
using AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails;
using AccommodationBooking.Application.Users.Queries.GetUser;
using AccommodationBooking.Application.Users.Queries.GetUsers;
using AccommodationBooking.Contracts.Users;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    /// <summary>
    /// Controller for managing user accounts.
    /// </summary>
    [Route("api/users")]
    public class UserController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Updates user's personal details.
        /// </summary>
        [HttpPost("{id:guid}/update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsRequest request, Guid id)
        {
            if (!CheckUserPermissions(id, out var errorResult))
                return errorResult;

            var query = _mapper.Map<UpdatePersonalDetailsCommand>((request, id));
            var result = await _mediator.Send(query);

            return result.Match(
                success => Ok(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Deletes a guest account.
        /// </summary>
        [HttpDelete("delete-guest/{id:guid}")]
        [Authorize(Roles = "Admin, Guest")]
        public async Task<IActionResult> DeleteGuest(Guid id)
        {
            if (!CheckUserPermissions(id, out var errorResult))
                return errorResult;

            var command = new DeleteGuestCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Deletes a host account.
        /// </summary>
        [HttpDelete("delete-host/{id:guid}")]
        [Authorize(Roles = "Admin, Host")]
        public async Task<IActionResult> DeleteHost(Guid id)
        {
            if (!CheckUserPermissions(id, out var errorResult))
                return errorResult;

            var command = new DeleteHostCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Deletes an admin account.
        /// </summary>
        [HttpDelete("delete-admin/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            if (!CheckUserPermissions(id, out var errorResult))
                return errorResult;

            var command = new DeleteAdminCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Gets all users with optional role filtering.
        /// </summary>
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

        /// <summary>
        /// Gets a specific user by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            if (!CheckUserPermissions(id, out var errorResult))
                return errorResult;

            var query = new GetUserQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                user => Ok(_mapper.Map<UserResponse>(user)),
                errors => Problem(errors));
        }

        private bool CheckUserPermissions(Guid resourceUserId, out IActionResult errorResult)
        {
            errorResult = null!;
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var tokenUserId))
            {
                errorResult = Unauthorized("Sesja wygasła.");
                return false;
            }

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (!isAdmin && tokenUserId != resourceUserId)
            {
                errorResult = Forbid("Nie posiadasz uprawnień do wykonania tej operacji na innym użytkowniku.");
                return false;
            }

            return true;
        }
    }
}