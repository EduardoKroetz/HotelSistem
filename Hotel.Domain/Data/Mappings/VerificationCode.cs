using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class VerificationCodeMapping : EntityBaseMapping<VerificationCode> , IEntityTypeConfiguration<VerificationCode>
{
  public void Configure(EntityTypeBuilder<VerificationCode> builder)
  {
    BaseMapping(builder);

    builder.ToTable("VerificationCodes");

    builder.Property(x => x.Code)
      .HasColumnType("NVARCHAR")
      .HasMaxLength(40);

    builder.OwnsOne(x => x.Email, e =>
    {
      e.Ignore(e => e.IsValid);

      e.Property<string>(x => x.Address)
        .IsRequired()
        .HasColumnName("Email")
        .HasColumnType("VARCHAR(120)");

      e.HasIndex(x => x.Address)
        .IsUnique();
    });

    builder.OwnsOne(x => x.Phone, p =>
    {
      p.Ignore(p => p.IsValid);

      p.Property<string>(x => x.Number)
        .IsRequired()
        .HasColumnName("Phone")
        .HasColumnType("VARCHAR(50)");

      p.HasIndex(x => x.Number)
        .IsUnique();
    });

    builder.HasIndex(x => x.Code)
      .IsUnique();
  }
}
