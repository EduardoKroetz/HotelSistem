using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.DislikeEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class DislikeMapping : EntityBaseMapping<Dislike>, IEntityTypeConfiguration<Dislike>
{
    public void Configure(EntityTypeBuilder<Dislike> builder)
    {
        BaseMapping(builder);

        builder.HasOne(x => x.Customer)
          .WithMany(x => x.Dislikes)
          .HasForeignKey(x => x.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Feedback)
          .WithMany(x => x.Dislikes)
          .HasForeignKey(x => x.FeedbackId)
          .OnDelete(DeleteBehavior.Restrict);

    }
}
