using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleUpdateNumberAsync(Guid id, int newNumber)
  {
    var room = await _repository.GetEntityByIdAsync(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    room.ChangeNumber(newNumber);

    await _repository.SaveChangesAsync();

    return new Response(200, "Número atualizado com sucesso!");
  }
}