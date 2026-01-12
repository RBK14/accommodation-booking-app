using AccommodationBooking.Domain.HostProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace AccommodationBooking.Infrastructure.Persistence.Configurations
{
    public class HostProfileConfiguration : IEntityTypeConfiguration<HostProfile>
    {
        public void Configure(EntityTypeBuilder<HostProfile> builder)
        {
            builder.ToTable("HostProfiles");

            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id)
                .ValueGeneratedNever();

            builder.Property(h => h.UserId)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasIndex(h => h.UserId);

            builder.Property(h => h.ListingIds)
                .HasField("_listingIds")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>()
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            builder.Property(h => h.ListingIds)
                .HasColumnType("nvarchar(max)");

            builder.Property(h => h.CreatedAt)
                .IsRequired();

            builder.Property(h => h.UpdatedAt)
                .IsRequired();
        }
    }
}