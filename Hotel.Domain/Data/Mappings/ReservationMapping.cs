using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.ReservationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hotel.Domain.Data.Mappings;

public class ReservationMapping : EntityBaseMapping<Reservation>, IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        BaseMapping(builder);

        builder.ToTable("Reservations");

        builder.Property(x => x.ExpectedCheckIn);

        builder.Property(x => x.ExpectedCheckOut);

        builder.Property(x => x.ExpectedTimeHosted)
          .IsRequired(true)
          .HasConversion(new TimeSpanToTicksConverter());

        builder.Property(x => x.TimeHosted)
          .IsRequired(false)
          .HasConversion(new TimeSpanToTicksConverter());

        builder.Property(x => x.DailyRate)
          .IsRequired()
          .HasColumnType("DECIMAL(18,2)");
        ;

        builder.Property(x => x.CheckIn);

        builder.Property(x => x.CheckOut)
          .IsRequired(false);

        builder.Property(x => x.Status)
          .HasConversion<int>();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.StripePaymentIntentId)
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

        builder.HasOne(x => x.Customer)
          .WithMany(x => x.Reservations)
          .HasForeignKey(x => x.CustomerId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Services)
          .WithMany(x => x.Reservations)
          .UsingEntity(j => j.ToTable("ReservationServices"));


    }
}