﻿using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleUpdatePriceAsync(Guid id, decimal price)
  {
    var room = await _repository.GetRoomIncludesReservations(id)
      ?? throw new ArgumentException("Cômodo não encontrado.");

    var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
    if (pendingReservations.Count > 0 && price != room.Price)
      throw new InvalidOperationException("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas ao cômodo.");

    room.ChangePrice(price);

    await _repository.SaveChangesAsync();

    return new Response(200, "Preço atualizado com sucesso!");
  }
}