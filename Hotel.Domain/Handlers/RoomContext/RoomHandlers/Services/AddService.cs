using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleAddServiceAsync(Guid id, Guid serviceId)
  {
    var room = await _repository.GetRoomIncludesServices(id);
    if (room == null)
      throw new NotFoundException("Cômodo não encontrada.");

    var service = await _serviceRepository.GetEntityByIdAsync(serviceId);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    room.AddService(service);

    await _repository.SaveChangesAsync();

    return new Response(200, "Serviço adicinado com sucesso!.");
  }
}