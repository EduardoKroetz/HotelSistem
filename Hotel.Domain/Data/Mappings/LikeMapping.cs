using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.LikeEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class LikeMapping : EntityBaseMapping<Like>, IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        BaseMapping(builder);

        builder.HasOne(x => x.Customer)
          .WithMany(x => x.Likes)
          .HasForeignKey(x => x.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Feedback)
          .WithMany(x => x.Likes)
          .HasForeignKey(x => x.FeedbackId)
          .OnDelete(DeleteBehavior.Restrict);

    }
}
