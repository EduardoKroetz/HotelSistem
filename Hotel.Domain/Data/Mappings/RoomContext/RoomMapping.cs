using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.RoomContext;

public class RoomMapping : EntityBaseMapping<Room>, IEntityTypeConfiguration<Room>
{
  public void Configure(EntityTypeBuilder<Room> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Rooms");

    builder.Property(x => x.Number);

    builder.Property(x => x.Price);

    builder.Property(x => x.Status)
      .HasConversion<int>();

    builder.Property(x => x.Capacity);

    builder.Property(x => x.Description);

    builder.Property(x => x.CategoryId);
    builder.HasOne(x => x.Category)
      .WithMany()
      .HasForeignKey(x => x.CategoryId)
      .HasConstraintName("FK_Rooms_Category")
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(x => x.Services)
      .WithMany()
      .UsingEntity(j => j.ToTable("RoomServices"));

    builder.HasMany(x => x.Images)
      .WithOne()
      .HasForeignKey(x => x.RoomId)
      .HasConstraintName("RoomImages");

  }
}