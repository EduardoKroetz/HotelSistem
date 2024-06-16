using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleRemoveServiceAsync(Guid id, Guid serviceId)
  {
    var room = await _repository.GetRoomIncludesServices(id)
      ?? throw new NotFoundException("Cômodo não encontrado.");

    var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
      ?? throw new NotFoundException("Serviço não encontrado.");

    room.RemoveService(service);

    await _repository.SaveChangesAsync();

    return new Response(200, "Serviço removido com sucesso!");
  }
}