using AccommodationBooking.Domain.GuestProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
namespace AccommodationBooking.Infrastructure.Persistence.Configurations
{
    public class GuestProfileConfiguration : IEntityTypeConfiguration<GuestProfile>
    {
        public void Configure(EntityTypeBuilder<GuestProfile> builder)
        {
            builder.ToTable("GuestProfiles");

            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).ValueGeneratedNever();

            builder.Property(g => g.UserId)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasIndex(g => g.UserId);

            builder.Property(g => g.CreatedAt).IsRequired();
            builder.Property(g => g.UpdatedAt).IsRequired();

            builder.Property(g => g.ReservationIds)
                .HasField("_reservationIds")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>()
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            builder.Property(g => g.ReservationIds).HasColumnType("nvarchar(max)");
        }
    }
}
