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
    [Route("api/reservations")]
    public class ReservationController(
        ISender mediator,
        IMapper mapper) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> CreateReservation(CreateReservationRequest request)
        {
            var profileIdValue = User.FindFirstValue("ProfileId");
            if (!Guid.TryParse(profileIdValue, out var profileId))
                return Unauthorized("Sesja wygasła. Zaloguj się ponownie.");

            Console.WriteLine(request);

            var command = _mapper.Map<CreateReservationCommand>((request, profileId));
            var result = await _mediator.Send(command);

            return result.Match(
                reservation => Ok(_mapper.Map<ReservationResponse>(reservation)),
                errors => Problem(errors));
        }

        [HttpPost("{id:guid}")]
        [Authorize(Roles = "Admin, Host")]
        public async Task<IActionResult> UpdateStatus(UpdateReservationStatusRequest request, Guid id)
        {
            if (id == Guid.Empty)
                return ValidationProblem("Identyfikator rezerwacji jest nieprawidłowy.");

            var roleValue = User.FindFirstValue(ClaimTypes.Role);
            var isGuest = roleValue.IsInRole(UserRole.Guest);
            var isHost = roleValue.IsInRole(UserRole.Host);

            ReservationStatusExtensions.TryParse(request.Status, out var status);

            if ((isGuest || isHost) && status != ReservationStatus.Cancelled)
                return Forbid("Nie posiadasz uprawnień do zmiany statusu na anulowana.");

            if (isHost && status != ReservationStatus.NoShow)
                return Forbid("Nie posiadasz uprawnień do zmiany statusu na nieobyta.");

            var command = _mapper.Map<UpdateReservationStatusCommand>((request, id));
            var result = await _mediator.Send(command);

            return result.Match(
                reservation => Ok(_mapper.Map<ReservationResponse>(reservation)),
                errors => Problem(errors));
        }

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
