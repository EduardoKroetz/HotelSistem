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

    builder.Property(x => x.Summary)
      .IsRequired();

    builder.Property(x => x.Description)
      .IsRequired();

    builder.Property(x => x.Status)
      .IsRequired()
      .HasConversion<int>();

    builder.Property(x => x.Priority)
      .IsRequired()
      .HasConversion<int>();

    builder.Property(x => x.Resolution)
      .IsRequired();

    builder.Property(x => x.EmployeeId)
      .IsRequired();

    builder.HasOne(x => x.Employee)
      .WithMany()
      .HasForeignKey(x => x.EmployeeId)
      .IsRequired()
      .HasConstraintName("FK_Reports_Employee")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
