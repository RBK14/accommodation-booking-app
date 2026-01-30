using AccommodationBooking.Application.Listings.Commands.DeleteReview;
using AccommodationBooking.Application.Reservations.Commands.CreateReservation;
using AccommodationBooking.Application.Reservations.Commands.DeleteReservation;
using AccommodationBooking.Application.Reservations.Commands.UpdateReservationStatus;
using AccommodationBooking.Application.Reservations.Queries.GetReservation;
using AccommodationBooking.Application.Reservations.Queries.GetReservations;
using AccommodationBooking.Contracts.Reservations;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.UserAggregate.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccommodationBooking.Api.Controllers
{
    /// <summary>
    /// Controller for managing reservations.
    /// </summary>
    [Route("api/reservations")]
    public class ReservationController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> CreateReservation(CreateReservationRequest request)
        {
            var profileIdValue = User.FindFirstValue("ProfileId");
            if (!Guid.TryParse(profileIdValue, out var profileId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            var command = _mapper.Map<CreateReservationCommand>((request, profileId));
            var result = await _mediator.Send(command);

            return result.Match(
                reservation => Ok(_mapper.Map<ReservationResponse>(reservation)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Updates the status of a reservation.
        /// </summary>
        [HttpPost("{id:guid}")]
        [Authorize(Roles = "Admin, Host, Guest")]
        public async Task<IActionResult> UpdateStatus(UpdateReservationStatusRequest request, Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator rezerwacji jest nieprawidłowy.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isGuest = roleValue.IsInRole(UserRole.Guest);
            var isHost = roleValue.IsInRole(UserRole.Host);
            var isAdmin = roleValue.IsInRole(UserRole.Admin);

            if (!ReservationStatusExtensions.TryParse(request.Status, out var status))
                return BadRequest("Nieprawidłowy status rezerwacji.");

            if (isGuest && status != ReservationStatus.Cancelled)
                return StatusCode(403, "Gość może tylko anulować rezerwację.");

            if (isHost && status != ReservationStatus.Cancelled && status != ReservationStatus.NoShow)
                return StatusCode(403, "Gospodarz może tylko anulować rezerwację lub oznaczyć jako nieodbyta.");

            var command = _mapper.Map<UpdateReservationStatusCommand>((request, id));
            var result = await _mediator.Send(command);

            return result.Match(
                reservation => Ok(_mapper.Map<ReservationResponse>(reservation)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Deletes a reservation.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReservation(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator rezerwacji jest nieprawidłowy.");

            var command = new DeleteReservationCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Gets reservations with optional filtering.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetReservations(Guid? listingId, Guid? guestProfileId, Guid? hostProfileId)
        {
            var query = new GetReservationsQuery(listingId, guestProfileId, hostProfileId);
            var result = await _mediator.Send(query);

            var response = result
                .Select(reservation => _mapper.Map<ReservationResponse>(reservation))
                .ToList();

            return Ok(response);
        }

        /// <summary>
        /// Gets a specific reservation by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReservation(Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator rezerwacji jest nieprawidłowy.");

            var query = new GetReservationQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(
                reservation => Ok(_mapper.Map<ReservationResponse>(reservation)),
                errors => Problem(errors));
        }
    }
}
