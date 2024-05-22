using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface IRoomRepository : IRepository<Room>, IRepositoryQuery<GetRoom, GetRoomCollection, RoomQueryParameters>
{
  public Task<Room?> GetRoomIncludeServices(Guid roomId);
}