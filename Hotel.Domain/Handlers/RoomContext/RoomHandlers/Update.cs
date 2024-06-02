using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler 
{
  public async Task<Response> HandleUpdateAsync(EditorRoom model, Guid id)
  {
    var room = await _repository.GetRoomIncludesPendingReservations(id);
    if (room == null)
      throw new NotFoundException("Cômodo não encontrado.");

    if (room.Reservations.Count > 0 && model.Price != room.Price)
      throw new InvalidOperationException("Não é possível atualizar o preço quando possuem reservas pendentes relacionadas ao cômodo.");

    room.ChangeNumber(model.Number);
    room.ChangeCapacity(model.Capacity);
    room.ChangePrice(model.Price);
    room.ChangeDescription(model.Description);
    room.ChangeCategory(model.CategoryId);

    _repository.Update(room);
    await _repository.SaveChangesAsync();

    return new Response(200,"Cômodo atualizado com sucesso!",new { room.Id });
  }
}