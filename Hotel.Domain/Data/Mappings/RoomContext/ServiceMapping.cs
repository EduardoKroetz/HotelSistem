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

    builder.Property(x => x.Name)
      .IsRequired();;

    builder.HasIndex(x => x.Name)
      .IsUnique();

    builder.Property(x => x.Price)
      .IsRequired()
      .HasColumnType("DECIMAL(18,2)");;

    builder.Property(x => x.IsActive)
      .IsRequired();

    builder.Property(x => x.Priority)
      .IsRequired()
      .HasConversion<int>();

    builder.Property(x => x.TimeInMinutes)
      .IsRequired();

    builder.HasMany(x => x.Responsibilities)
      .WithMany(x => x.Services)
      .UsingEntity(j => j.ToTable("ServiceResponsibilities"));
  }
}

