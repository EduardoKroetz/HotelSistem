using Hotel.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.Base;

public class UserBaseMapping<T> : EntityBaseMapping<T> where T : User
{
  public override void BaseMapping(EntityTypeBuilder<T> builder)
  {
    base.BaseMapping(builder);
  
    builder.Property(x => x.Name.FirstName)
      .IsRequired()
      .HasColumnName("FirstName")
      .HasColumnType("VARCHAR")
      .HasMaxLength(20);

    builder.Property(x => x.Name.LastName)
      .IsRequired()
      .HasColumnName("LastName")
      .HasColumnType("VARCHAR")
      .HasMaxLength(20);

    builder.HasIndex(x => x.Email.Address)
      .IsUnique();

    builder.Property(x => x.Email.Address)
      .IsRequired()
      .HasColumnName("Email")
      .HasColumnType("NVARCHAR")
      .HasMaxLength(150);

    builder.HasIndex(x => x.Phone.Number)
      .IsUnique();

    builder.Property(x => x.Phone.Number)
      .IsRequired()
      .HasColumnName("Phone")
      .HasColumnType("NVARCHAR")
      .HasMaxLength(90);

    builder.Property(x => x.PasswordHash)
      .IsRequired()
      .HasColumnName("PasswordHash")
      .HasColumnType("NVARCHAR")
      .HasMaxLength(120);

    builder.Property(x => x.Gender)
      .IsRequired(false)
      .HasColumnName("Gender")
      .HasConversion<int>();
      
    builder.Property(x => x.DateOfBirth)
      .IsRequired(false)
      .HasColumnName("DateOfBirth")
      .HasColumnType("Date");

    builder.Property(x => x.Address.Country)
      .IsRequired(false)
      .HasColumnName("Country")
      .HasColumnType("VARCHAR")
      .HasMaxLength(60);

    builder.Property(x => x.Address.City)
      .IsRequired(false)
      .HasColumnName("City")
      .HasColumnType("VARCHAR")
      .HasMaxLength(60);

    builder.Property(x => x.Address.Number)
      .IsRequired(false)
      .HasColumnName("Number")
      .HasColumnType("SMALLINT");
  
  }
}