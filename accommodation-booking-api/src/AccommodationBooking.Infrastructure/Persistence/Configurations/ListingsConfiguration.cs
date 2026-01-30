using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.ListingAggregate;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace AccommodationBooking.Infrastructure.Persistence.Configurations
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            ConfigureListingTable(builder);
            ConfigureReviewsTable(builder);
            ConfigureScheduleSlotsTable(builder);
        }

        private void ConfigureListingTable(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable("Listings");

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedNever();

            builder.Property(l => l.HostProfileId)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasIndex(l => l.HostProfileId);

            builder.Property(l => l.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(l => l.AccommodationType)
                .HasConversion(
                    role => role.ToString(),
                    value => AccommodationTypeExtensions.Parse(value)
                )
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(l => l.Beds).IsRequired();
            builder.Property(l => l.MaxGuests).IsRequired();

            builder.Property(l => l.CreatedAt).IsRequired();
            builder.Property(l => l.UpdatedAt).IsRequired();

            builder.OwnsOne(l => l.Address, ab =>
            {
                ab.Property(a => a.Country)
                    .HasMaxLength(100)
                    .HasColumnName("Country")
                    .IsRequired();

                ab.Property(a => a.City)
                    .HasMaxLength(100)
                    .HasColumnName("City")
                    .IsRequired();

                ab.Property(a => a.PostalCode)
                    .HasMaxLength(10)
                    .HasColumnName("PostalCode")
                    .IsRequired();

                ab.Property(a => a.Street)
                    .HasMaxLength(150)
                    .HasColumnName("Street")
                    .IsRequired();

                ab.Property(a => a.BuildingNumber)
                    .HasMaxLength(20)
                    .HasColumnName("BuildingNumber")
                    .IsRequired();
            });

            builder.OwnsOne(o => o.PricePerDay, pb =>
            {
                pb.Property(p => p.Amount)
                    .HasColumnName("PriceAmount")
                    .HasPrecision(18, 2);

                pb.Property(p => p.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasConversion(
                        currency => currency.ToString(),
                        value => CurrencyExtensions.Parse(value)
                    )
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            builder.Property(l => l.ReservationIds)
                .HasField("_reservationIds")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>()
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            builder.Property(l => l.PhotoUrls)
                .HasField("_photoUrls")
                .HasColumnName("PhotoUrls")
                .IsRequired(false)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        }

        private void ConfigureReviewsTable(EntityTypeBuilder<Listing> builder)
        {
            builder.OwnsMany(l => l.Reviews, rb =>
            {
                rb.ToTable("Reviews");
                rb.WithOwner().HasForeignKey(r => r.ListingId);
                rb.HasKey("Id");
                rb.Property(r => r.Id).ValueGeneratedNever();

                rb.Property(r => r.GuestProfileId).IsRequired();
                rb.Property(r => r.Rating).IsRequired();
                rb.Property(r => r.Comment).HasMaxLength(1000);
            });

            builder.Metadata.FindNavigation(nameof(Listing.Reviews))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureScheduleSlotsTable(EntityTypeBuilder<Listing> builder)
        {
            builder.OwnsMany(l => l.ScheduleSlots, sb =>
            {
                sb.ToTable("ScheduleSlots");
                sb.WithOwner().HasForeignKey("ListingId");
                sb.HasKey("Id");
                sb.Property("Id").ValueGeneratedNever();

                sb.Property(s => s.ReservationId).IsRequired();
                sb.Property(s => s.StartDate).IsRequired();
                sb.Property(s => s.EndDate).IsRequired();

                sb.HasIndex(s => new { s.StartDate, s.EndDate });
            });

            builder.Metadata.FindNavigation(nameof(Listing.ScheduleSlots))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
