using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room>, IRepositoryQuery<GetRoom, RoomQueryParameters>
{
    Task<Room?> GetRoomIncludesServices(Guid roomId);
    Task<Room?> GetRoomIncludesReservations(Guid roomId);
}