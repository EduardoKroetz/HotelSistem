using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.CustomerEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hotel.Domain.Data.Mappings;

public class CustomerMapping : UserBaseMapping<Customer>, IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        BaseMapping(builder);


        builder.ToTable("Customers");

        builder.Property(x => x.StripeCustomerId)
            .HasColumnType("NVARCHAR(120)");

        builder.HasMany(c => c.Feedbacks)
          .WithOne(f => f.Customer)
          .HasForeignKey(f => f.CustomerId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
    }
}

