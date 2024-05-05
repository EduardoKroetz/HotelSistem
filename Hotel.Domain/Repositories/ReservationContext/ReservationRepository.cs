using Hotel.Domain.Data;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class ReservationRepository :  GenericRepository<Reservation> ,IReservationRepository
{
  public ReservationRepository(HotelDbContext context) : base(context) {}

}