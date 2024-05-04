using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.RoomContext;

public class ServiceMapping : EntityBaseMapping<Service>, IEntityTypeConfiguration<Service>
{
  public void Configure(EntityTypeBuilder<Service> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Services");

    builder.Property(x => x.Name);

    builder.Property(x => x.Price);

    builder.Property(x => x.IsActive);

    builder.Property(x => x.Priority)
      .HasConversion<int>();

    builder.Property(x => x.TimeInMinutes);

    builder.HasMany(x => x.Responsabilities)
      .WithMany()
      .UsingEntity(j => j.ToTable("ServiceResponsabilities"));
  }
}

