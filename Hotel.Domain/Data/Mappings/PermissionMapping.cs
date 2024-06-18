using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.PermissionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionMapping : EntityBaseMapping<Permission>, IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        BaseMapping(builder);

        builder.Property(p => p.Name)
          .IsRequired()
          .HasColumnType("VARCHAR")
          .HasMaxLength(100);

        builder.HasIndex(p => p.Name)
          .IsUnique();

        builder.Property(p => p.Description)
          .IsRequired()
          .HasColumnType("VARCHAR")
          .HasMaxLength(255);

        builder.Property(p => p.IsActive)
          .IsRequired()
          .HasColumnType("BIT");

    }
}
