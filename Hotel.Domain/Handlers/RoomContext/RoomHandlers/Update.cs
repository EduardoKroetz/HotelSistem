using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler 
{
  public async Task<Response> HandleUpdateAsync(EditorRoom model, Guid id)
  {
    var room = await _repository.GetEntityByIdAsync(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    room.ChangeNumber(model.Number);
    room.ChangeCapacity(model.Capacity);
    room.ChangePrice(model.Price);
    room.ChangeDescription(model.Description);
    room.ChangeCategory(model.CategoryId);

    _repository.Update(room);
    await _repository.SaveChangesAsync();

    return new Response(200,"Hospedagem atualizada com sucesso!",new { room.Id });
  }
}