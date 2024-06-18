using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleAddServiceAsync(Guid id, Guid serviceId)
    {
        var room = await _repository.GetRoomIncludesServices(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
          ?? throw new NotFoundException("Serviço não encontrado.");

        room.AddService(service);

        await _repository.SaveChangesAsync();

        return new Response(200, "Serviço adicinado com sucesso!");
    }
}