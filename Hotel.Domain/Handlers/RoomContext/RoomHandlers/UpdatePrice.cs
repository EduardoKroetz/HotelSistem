using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleUpdatePriceAsync(Guid id, decimal price)
  {
    var room = await _repository.GetEntityByIdAsync(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    room.ChangePrice(price);

    await _repository.SaveChangesAsync();

    return new Response(200, "Preço atualizado.");
  }
}