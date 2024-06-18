using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class AdminMapping : UserBaseMapping<Admin>, IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");

        base.BaseMapping(builder);

        builder.Property(x => x.IsRootAdmin)
          .IsRequired();

        builder.HasMany(x => x.Permissions)
          .WithMany(x => x.Admins)
          .UsingEntity<Dictionary<string, object>>
          (
            "AdminPermissions",
            j => j
              .HasOne<Permission>()
              .WithMany()
              .HasForeignKey("PermissionId")
              .HasConstraintName("FK_AdminPermission_Permission")
              .OnDelete(DeleteBehavior.Cascade),
            j => j
              .HasOne<Admin>()
              .WithMany()
              .HasForeignKey("AdminId")
              .HasConstraintName("FK_AdminPermissions_Admin")
              .OnDelete(DeleteBehavior.Cascade)
          );

    }
}