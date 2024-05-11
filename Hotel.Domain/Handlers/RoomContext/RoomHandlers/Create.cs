using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler : IHandler
{
  private readonly IRoomRepository  _repository;
  public RoomHandler(IRoomRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(EditorRoom model)
  {
    var room = new Room(model.Number,model.Price,model.Capacity,model.Description,model.CategoryId);  

    await _repository.CreateAsync(room);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Hospedagem criada.",new { room.Id });
  }
}