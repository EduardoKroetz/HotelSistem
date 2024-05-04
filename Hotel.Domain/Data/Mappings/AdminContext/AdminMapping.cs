using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.AdminContext;

public class AdminMapping : UserBaseMapping<Admin> ,IEntityTypeConfiguration<Admin>
{
  public void Configure(EntityTypeBuilder<Admin> builder)
  {
    builder.ToTable("Admins");

    builder.Property(x => x.IsRootAdmin)
      .IsRequired()
      .HasColumnName("IsRootAdmin")
      .HasColumnType("BIT");

    builder.HasMany(x => x.Permissions)
      .WithMany()
      .UsingEntity<Dictionary<string,object>>
      (
        "AdminPermissions",
        j => j
          .HasOne<Permission>()
          .WithMany()
          .HasForeignKey("PermissionId")
          .HasConstraintName("FK_AdminPermission_Permission")
          .OnDelete(DeleteBehavior.SetNull),
        j => j
          .HasOne<Admin>()
          .WithMany()
          .HasForeignKey("AdminId")
          .HasConstraintName("FK_AdminPermissions_Admin")
          .OnDelete(DeleteBehavior.SetNull),
        j =>
        {
          j.ToTable("AdminPermissions");

          j.Property<Guid>("AdminId")
            .HasColumnType("UNIQUEIDENTIFIER");

          j.Property<Guid>("PermissionId")
            .HasColumnType("UNIQUEIDENTIFIER");

          j.HasKey("AdminId", "PermissionId");
        }
      );

    BaseMapping(builder);
  }
}