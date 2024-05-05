using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class RoomRepository :  GenericRepository<Room> ,IRoomRepository
{
  public RoomRepository(HotelDbContext context) : base(context) {}

}