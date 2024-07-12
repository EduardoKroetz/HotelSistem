using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleRemoveServiceAsync(Guid id, Guid serviceId)
    {
        var room = await _repository.GetRoomIncludesServices(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
          ?? throw new NotFoundException("Serviço não encontrado.");

        room.RemoveService(service);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao desassociar serviço {service.Id} do cômodo {room.Id}. Erro: {e.Message}");
        }

        return new Response("Serviço removido com sucesso!");
    }
}