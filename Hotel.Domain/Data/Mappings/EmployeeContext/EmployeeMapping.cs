using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hotel.Domain.Data.Mappings.EmployeeContext;
public class EmployeeMapping : UserBaseMapping<Employee>, IEntityTypeConfiguration<Employee>
{
  public void Configure(EntityTypeBuilder<Employee> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Employees");

    builder.Property(e => e.Salary)
      .HasColumnType("DECIMAL(18,2)"); 

    builder.HasMany(e => e.Responsabilities)
      .WithMany()
      .UsingEntity<Dictionary<string,object>>
      (
        "EmployeeResponsabilities",
        j => j
          .HasOne<Responsability>()
          .WithMany()
          .HasForeignKey("ResponsabilityId")
          .IsRequired()
          .HasConstraintName("FK_EmployeeResponsabilities_Responsability")
          .OnDelete(DeleteBehavior.Cascade),
        j => j
          .HasOne<Employee>()
          .WithMany()
          .HasForeignKey("EmployeeId")
          .IsRequired()
          .HasConstraintName("FK_EmployeeResponsabilities_Employee")
          .OnDelete(DeleteBehavior.Cascade)
      );
      
  }
}

