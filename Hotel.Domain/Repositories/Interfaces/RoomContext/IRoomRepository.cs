using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room>, IRepositoryQuery<GetRoom,GetRoomCollection>
{
}