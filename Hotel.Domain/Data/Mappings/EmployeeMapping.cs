using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hotel.Domain.Data.Mappings;
public class EmployeeMapping : UserBaseMapping<Employee>, IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        BaseMapping(builder);

        builder.ToTable("Employees");

        builder.Property(e => e.Salary)
          .HasColumnType("DECIMAL(18,2)");

        builder.HasMany(x => x.Reports)
          .WithOne(x => x.Employee)
          .HasForeignKey(x => x.EmployeeId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Permissions)
          .WithMany(x => x.Employees)
          .UsingEntity(x => x.ToTable("EmployeePermissions"));

        builder.HasMany(e => e.Responsibilities)
          .WithMany(x => x.Employees)
          .UsingEntity<Dictionary<string, object>>
          (
            "EmployeeResponsibilities",
            j => j
              .HasOne<Responsibility>()
              .WithMany()
              .HasForeignKey("ResponsibilityId")
              .HasConstraintName("FK_EmployeeResponsibilities_Responsibility")
              .OnDelete(DeleteBehavior.Cascade),
            j => j
              .HasOne<Employee>()
              .WithMany()
              .HasForeignKey("EmployeeId")
              .HasConstraintName("FK_EmployeeResponsibilities_Employee")
              .OnDelete(DeleteBehavior.Cascade)
          );

    }
}

