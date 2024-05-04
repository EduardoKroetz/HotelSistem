using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.CustomerContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hotel.Domain.Data.Mappings.CustomerContext;

public class CustomerMapping : UserBaseMapping<Customer>, IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Customers");

    builder.HasMany(c => c.Feedbacks)
      .WithOne(f => f.Customer)
      .HasForeignKey(f => f.CustomerId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);
  }
}

