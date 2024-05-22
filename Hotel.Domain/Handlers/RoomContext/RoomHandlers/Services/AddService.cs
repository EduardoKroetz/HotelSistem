using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response<object>> HandleAddServiceAsync(Guid id, Guid serviceId)
  {
    var room = await _repository.GetRoomIncludeServices(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    var service = await _serviceRepository.GetEntityByIdAsync(serviceId);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    room.AddService(service);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Serviço adicinado.");
  }
}