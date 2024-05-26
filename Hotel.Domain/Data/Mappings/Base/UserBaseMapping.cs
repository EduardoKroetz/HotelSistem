using Hotel.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.Base;

public class UserBaseMapping<T> : EntityBaseMapping<T> where T : User
{
  public override void BaseMapping(EntityTypeBuilder<T> builder)
  {
    base.BaseMapping(builder);

    builder.OwnsOne(x => x.Address, a =>
    {
      a.Ignore(x => x.IsValid);

      a.Property<string?>(x => x.Country)
        .IsRequired(false)
        .HasColumnName("Country")
        .HasColumnType("VARCHAR(40)");

      a.Property<string?>(x => x.City)
        .IsRequired(false)
        .HasColumnName("City")
        .HasColumnType("VARCHAR(40)");

      a.Property<string?>(x => x.Street)
        .IsRequired(false)
        .HasColumnName("Street")
        .HasColumnType("VARCHAR(40)");

      a.Property<int?>(x => x.Number)
        .HasColumnName("Number")
        .HasColumnType("INT")
        .HasDefaultValue(0);
    });

    builder.OwnsOne(x => x.Name, n => 
    {
      n.Ignore(x => x.IsValid);

      n.Property<string>(x => x.FirstName)
        .IsRequired()
        .HasColumnName("FirstName")
        .HasColumnType("VARCHAR(40)");

      n.Property<string>(x => x.LastName)
        .IsRequired()
        .HasColumnName("LastName")
        .HasColumnType("VARCHAR(40)");
    });


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
  
  }
}