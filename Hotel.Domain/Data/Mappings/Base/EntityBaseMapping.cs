
using Hotel.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.Base;

public class EntityBaseMapping<T> where T : Entity
{
    public virtual void BaseMapping(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsValid);

        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedNever()
          .HasColumnName("Id")
          .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(x => x.CreatedAt)
          .IsRequired()
          .HasColumnType("datetime")
          .HasColumnName("CreatedAt");
    }
}
