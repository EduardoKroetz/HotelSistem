using Hotel.Domain.Data.Mappings.Base;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Domain.Data.Mappings.CustomerContext;

public class FeedbackMapping : EntityBaseMapping<Feedback> ,IEntityTypeConfiguration<Feedback>
{
  public void Configure(EntityTypeBuilder<Feedback> builder)
  {
    BaseMapping(builder);

    builder.ToTable("Feedbacks");

    builder.Property(f => f.Comment)
      .IsRequired()
      .HasColumnType("VARCHAR")
      .HasMaxLength(255);

    builder.Property(f => f.Rate)
      .IsRequired()
      .HasColumnType("SMALLINT");

    builder.Property(f => f.Likes)
      .IsRequired()
      .HasColumnType("SMALLINT");;

    builder.Property(f => f.Deslikes)
      .IsRequired()
      .HasColumnType("SMALLINT");;

    builder.Property(f => f.UpdatedAt)
      .IsRequired()
      .HasColumnType("DATETIME");

    builder.HasOne(f => f.Reservation)
      .WithMany()
      .HasForeignKey(f => f.ReservationId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(f => f.Room)
      .WithMany()
      .HasForeignKey(f => f.RoomId)
      .IsRequired() 
      .OnDelete(DeleteBehavior.Cascade);
  }
}

