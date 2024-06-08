using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.CustomerContext.FeedbackContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.CustomerContext;

public class DeslikeMapping : EntityBaseMapping<Deslike>, IEntityTypeConfiguration<Deslike>
{
  public void Configure(EntityTypeBuilder<Deslike> builder)
  {
    BaseMapping(builder);

    builder.HasOne(x => x.Customer)
      .WithMany(x => x.Deslikes)
      .HasForeignKey(x => x.CustomerId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.Feedback)
      .WithMany(x => x.Deslikes)
      .HasForeignKey(x => x.FeedbackId)
      .OnDelete(DeleteBehavior.Restrict);

  }
}
