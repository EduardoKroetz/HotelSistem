using Hotel.Domain.DTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleChangeToAvailableStatusAsync(Guid id)
    {
        var room = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        room.ChangeStatus(ERoomStatus.Available);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao atualizar o status do cômodo {room.Id} para disponível no banco de dados. Erro: {e.Message}");
        }

        return new Response("Status atualizado com sucesso!");
    }
}