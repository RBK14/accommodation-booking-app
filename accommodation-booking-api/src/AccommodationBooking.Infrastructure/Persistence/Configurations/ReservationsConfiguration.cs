using AccommodationBooking.Domain.ReservationAggregate;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using AccommodationBooking.Domain.Common.ValueObjects; // Zakładam Address i Price
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccommodationBooking.Infrastructure.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Property(r => r.ListingId).IsRequired();
            builder.HasIndex(r => r.ListingId);

            builder.Property(r => r.GuestProfileId).IsRequired();
            builder.HasIndex(r => r.GuestProfileId);

            builder.Property(r => r.HostProfileId).IsRequired();
            builder.HasIndex(r => r.HostProfileId);

            builder.Property(r => r.ListingTitle)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(r => r.CheckIn).IsRequired();
            builder.Property(r => r.CheckOut).IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.UpdatedAt).IsRequired();

            builder.Property(r => r.Status)
                .HasConversion(
                    status => status.ToString(),
                    value => ReservationStatusExtensions.Parse(value)
                )
                .HasMaxLength(50)
                .IsRequired();

            builder.OwnsOne(r => r.ListingAddress, ab =>
            {
                ab.Property(a => a.Country).HasMaxLength(100).HasColumnName("ListingCountry").IsRequired();
                ab.Property(a => a.City).HasMaxLength(100).HasColumnName("ListingCity").IsRequired();
                ab.Property(a => a.PostalCode).HasMaxLength(10).HasColumnName("ListingPostalCode").IsRequired();
                ab.Property(a => a.Street).HasMaxLength(150).HasColumnName("ListingStreet").IsRequired();
                ab.Property(a => a.BuildingNumber).HasMaxLength(20).HasColumnName("ListingBuildingNumber").IsRequired();
            });

            builder.OwnsOne(r => r.ListingPricePerDay, pb =>
            {
                pb.Property(p => p.Amount)
                    .HasColumnName("ListingPriceAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                pb.Property(p => p.Currency)
                    .HasConversion<string>()
                    .HasMaxLength(3)
                    .HasColumnName("ListingPriceCurrency")
                    .IsRequired();
            });

            builder.OwnsOne(r => r.TotalPrice, pb =>
            {
                pb.Property(p => p.Amount)
                    .HasColumnName("TotalPriceAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                pb.Property(p => p.Currency)
                    .HasConversion<string>()
                    .HasMaxLength(3)
                    .HasColumnName("TotalPriceCurrency")
                    .IsRequired();
            });
        }
    }
}