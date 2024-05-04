using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.PaymentContext;

public class RoomInvoiceMapping : EntityBaseMapping<RoomInvoice>, IEntityTypeConfiguration<RoomInvoice>
{
  public void Configure(EntityTypeBuilder<RoomInvoice> builder)
  {
    BaseMapping(builder);

    builder.ToTable("RoomInvoices");

    builder.Property(x => x.Number)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(x => x.IssueDate)
        .IsRequired();

    builder.Property(x => x.TotalAmount)
        .IsRequired()
        .HasColumnType("DECIMAL(18,2)");;

    builder.Property(x => x.Status)
        .IsRequired()
        .HasConversion<int>(); 

    builder.Property(x => x.PaymentMethod)
        .IsRequired()
        .HasConversion<int>(); 

    builder.Property(x => x.TaxInformation)
        .IsRequired()
        .HasColumnType("DECIMAL(18,2)");

    builder.Property(x => x.ReservationId)
        .IsRequired()
        .HasColumnType("UNIQUEIDENTIFIER");

    builder.HasOne(x => x.Reservation)
        .WithOne(x => x.Invoice)
        .OnDelete(DeleteBehavior.SetNull);

    builder.HasMany(x => x.Customers)
        .WithMany(x => x.RoomInvoices)
        .UsingEntity(j => j.ToTable("CustomerRoomInvoices"));

    builder.HasMany(x => x.Services)
        .WithMany(x => x.RoomInvoices)
        .UsingEntity(j => j.ToTable("ServicesRoomInvoices"));

  }
}