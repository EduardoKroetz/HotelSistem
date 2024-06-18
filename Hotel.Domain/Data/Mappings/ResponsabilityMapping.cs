using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;
public class ResponsibilityMapping : EntityBaseMapping<Responsibility>, IEntityTypeConfiguration<Responsibility>
{
    public void Configure(EntityTypeBuilder<Responsibility> builder)
    {
        BaseMapping(builder);

        builder.ToTable("Responsibilities");

        builder.Property(r => r.Name)
          .IsRequired()
          .HasColumnType("VARCHAR")
          .HasMaxLength(100);

        builder.HasIndex(r => r.Name)
          .IsUnique();

        builder.Property(r => r.Description)
          .IsRequired()
          .HasColumnType("VARCHAR")
          .HasMaxLength(255);

        builder.Property(r => r.Priority)
          .IsRequired()
          .HasConversion<int>();

    }
}

