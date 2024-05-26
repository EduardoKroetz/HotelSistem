using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.RoomContext;

public class ImageMapping : EntityBaseMapping<Image>, IEntityTypeConfiguration<Image>
{
  public void Configure(EntityTypeBuilder<Image> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Images");

    builder.Property(x => x.Url)
      .IsRequired();

    builder.HasIndex(x => x.Url)
      .IsUnique();

    builder.Property(x => x.RoomId)
      .IsRequired();
  }
}