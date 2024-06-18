using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.InvoiceEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings;

public class InvoiceMapping : EntityBaseMapping<Invoice>, IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        BaseMapping(builder);

        builder.ToTable("Invoices");

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Number)
          .IsUnique();

        builder.Property(x => x.IssueDate)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");
        ;

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

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.CustomerId)
            .HasConstraintName("FK_Invoices_Customer")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Services)
            .WithMany(x => x.Invoices)
            .UsingEntity(j => j.ToTable("ServicesInvoices"));

    }
}