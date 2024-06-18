using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class RoomMapping : EntityBaseMapping<Room>, IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        BaseMapping(builder);

        builder.ToTable("Rooms");

        builder.Property(x => x.Number)
          .IsRequired();

        builder.HasIndex(x => x.Number)
          .IsUnique();

        builder.Property(x => x.IsActive)
          .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(x => x.Status)
          .HasConversion<int>();

        builder.Property(x => x.Capacity)
          .IsRequired();

        builder.Property(x => x.Description)
          .IsRequired();

        builder.Property(x => x.CategoryId);
        builder.HasOne(x => x.Category)
          .WithMany(x => x.Rooms)
          .HasForeignKey(x => x.CategoryId)
          .HasConstraintName("FK_Rooms_Category")
          .IsRequired()
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Services)
        .WithMany(s => s.Rooms)
        .UsingEntity<Dictionary<string, object>>(
          "RoomServices",
          r => r.HasOne<Service>()
            .WithMany()
            .HasForeignKey("ServiceId")
            .OnDelete(DeleteBehavior.Cascade),
          s => s.HasOne<Room>()
            .WithMany()
            .HasForeignKey("RoomId")
            .OnDelete(DeleteBehavior.Cascade)
        );

        builder.HasMany(x => x.Images)
          .WithOne(x => x.Room)
          .HasForeignKey(x => x.RoomId)
          .HasConstraintName("RoomImages")
          .OnDelete(DeleteBehavior.Cascade);

    }
}