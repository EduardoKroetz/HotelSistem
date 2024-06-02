using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface IRoomRepository : IRepository<Room>, IRepositoryQuery<GetRoom, GetRoomCollection, RoomQueryParameters>
{
  Task<Room?> GetRoomIncludesServices(Guid roomId);
  Task<Room?> GetRoomIncludesReservations(Guid roomId);
  Task<Room?> GetRoomIncludesPendingReservations(Guid roomId);
}