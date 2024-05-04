using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.RoomContext;

public class CategoryMapping : EntityBaseMapping<Category>, IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Categories");

    builder.Property(x => x.Name);

    builder.Property(x => x.Description);

    builder.Property(x => x.AveragePrice);
  }
}