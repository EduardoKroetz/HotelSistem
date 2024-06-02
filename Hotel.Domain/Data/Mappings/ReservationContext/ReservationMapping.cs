using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
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

    builder.Property(x => x.DailyRate)
      .IsRequired()
      .HasColumnType("DECIMAL(18,2)");;

    builder.Property(x => x.CheckIn);

    builder.Property(x => x.CheckOut)
      .IsRequired(false);

    builder.Property(x => x.Status)
      .HasConversion<int>();

    builder.Property(x => x.Capacity)
        .IsRequired();

    builder.Property(x => x.RoomId)
        .IsRequired();

    builder.Property(x => x.InvoiceId)
      .IsRequired(false);

    builder.HasOne(x => x.Invoice)
      .WithOne(x => x.Reservation)
      .HasForeignKey<Reservation>(x => x.InvoiceId)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasOne(x => x.Room)
      .WithMany(x => x.Reservations)
      .HasForeignKey(x => x.RoomId)
      .IsRequired()
      .HasConstraintName("FK_Reservations_Room")
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(x => x.Customers)
      .WithMany(x => x.Reservations)
      .UsingEntity(j => j.ToTable("ReservationCustomers"));

    builder.HasMany(x => x.Services)
      .WithMany(x => x.Reservations)
      .UsingEntity(j => j.ToTable("ReservationServices"));
    

  }
}