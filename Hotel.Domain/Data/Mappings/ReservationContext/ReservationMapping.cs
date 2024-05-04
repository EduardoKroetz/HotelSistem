using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.ReservationContext;

public class ReservationMapping : EntityBaseMapping<Reservation>, IEntityTypeConfiguration<Reservation>
{
  public void Configure(EntityTypeBuilder<Reservation> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Reservations");

    builder.Property(x => x.HostedDays)
      .IsRequired(false);

    builder.Property(x => x.DailyRate);

    builder.Property(x => x.CheckIn);

    builder.Property(x => x.CheckOut)
      .IsRequired(false);

    builder.Property(x => x.Status)
      .HasConversion<int>();

    builder.Property(x => x.Capacity);

    builder.Property(x => x.RoomId);

    builder.Property(x => x.InvoiceId)
      .IsRequired(false);

    builder.HasOne(x => x.Invoice)
      .WithOne(x => x.Reservation)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasOne(x => x.Room)
      .WithMany()
      .HasForeignKey(x => x.RoomId)
      .IsRequired()
      .HasConstraintName("FK_Reservations_Room")
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(x => x.Customers)
      .WithMany()
      .UsingEntity(j => j.ToTable("ReservationCustomers"));

    builder.HasMany(x => x.Services)
      .WithMany()
      .UsingEntity(j => j.ToTable("ReservationServices"));
    

  }
}