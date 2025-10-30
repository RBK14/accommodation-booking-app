﻿using AccommodationBooking.Domain.ListingAggregate;
using ErrorOr;
using MediatR;

namespace AccommodationBooking.Application.Listings.Commands
{
    public record CreateListingCommand(
        Guid HostProfileId,
        string Title,
        string Description,
        string AccommodationType,
        int Beds,
        int MaxGuests,
        string Country,
        string City,
        string PostalCode,
        string Street,
        string BuildingNumber,
        decimal AmountPerDay,
        string Currency) : IRequest<ErrorOr<Listing>>;
}
