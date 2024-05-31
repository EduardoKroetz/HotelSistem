using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class VerificationCodeMapping : IEntityTypeConfiguration<VerificationCode>
{
  public void Configure(EntityTypeBuilder<VerificationCode> builder)
  {
    builder.ToTable("VerificationCodes");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Code)
      .HasColumnType("NVARCHAR")
      .HasMaxLength(40);

    builder.HasIndex(x => x.Code)
      .IsUnique();
  }
}
