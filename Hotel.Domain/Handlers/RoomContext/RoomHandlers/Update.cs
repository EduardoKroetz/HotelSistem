using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler 
{
  public async Task<Response> HandleUpdateAsync(EditorRoom model, Guid id)
  {
    var room = await _repository.GetRoomIncludesReservations(id)
      ?? throw new NotFoundException("Hospedagem não encontrada.");

    var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
    if (pendingReservations.Count > 0 && model.Price != room.Price)
      throw new InvalidOperationException("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas a hospedagem.");

    room.ChangeNumber(model.Number);
    room.ChangeCapacity(model.Capacity);
    room.ChangePrice(model.Price);
    room.ChangeDescription(model.Description);
    room.ChangeCategory(model.CategoryId);

    try
    {
      _repository.Update(room);
      await _repository.SaveChangesAsync();
    }
    catch (DbUpdateException e)
    {
      if (e.InnerException != null && e.InnerException.ToString().Contains("Number"))
        throw new ArgumentException("Esse número da hospedagem já foi cadastrado.");
      else
        throw new Exception();
    }


    return new Response(200,"Hospedagem atualizada com sucesso!",new { room.Id });
  }
}