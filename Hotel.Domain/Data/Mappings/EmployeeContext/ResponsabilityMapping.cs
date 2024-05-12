using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.EmployeeContext;
public class ResponsabilityMapping : EntityBaseMapping<Responsability>, IEntityTypeConfiguration<Responsability>
{
  public void Configure(EntityTypeBuilder<Responsability> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Responsabilities");

    builder.Property(p => p.Name)
      .IsRequired()
      .HasColumnType("VARCHAR")
      .HasMaxLength(100);

    builder.Property(p => p.Description)
      .IsRequired()
      .HasColumnType("VARCHAR")
      .HasMaxLength(255);

    builder.Property(p => p.Priority)
      .IsRequired()
      .HasConversion<int>();

  }
}

