using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleUpdatePriceAsync(Guid id, decimal price)
  {
    var room = await _repository.GetRoomIncludesReservations(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    if (room.Reservations.Count > 0 )
      throw new InvalidOperationException("Não é possível atualizar o preço quando possuem reservas pendentes relacionadas ao cômodo.");

    room.ChangePrice(price);

    await _repository.SaveChangesAsync();

    return new Response(200, "Preço atualizado com sucesso!");
  }
}