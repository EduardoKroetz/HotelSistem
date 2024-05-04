using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.RoomContext;

public class ReportMapping : EntityBaseMapping<Report>, IEntityTypeConfiguration<Report>
{
  public void Configure(EntityTypeBuilder<Report> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Reports");

    builder.Property(x => x.Summary);

    builder.Property(x => x.Description);

    builder.Property(x => x.Status)
      .HasConversion<int>();

    builder.Property(x => x.Priority)
      .HasConversion<int>();

    builder.Property(x => x.Resolution);

    builder.Property(x => x.EmployeeId);

    builder.HasOne(x => x.Employee)
      .WithMany()
      .HasForeignKey(x => x.EmployeeId)
      .IsRequired()
      .HasConstraintName("FK_Reports_Employee")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
