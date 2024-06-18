using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.ReportEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

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
    }
}
