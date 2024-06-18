using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateNumberAsync(Guid id, int newNumber)
    {
        var room = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        room.ChangeNumber(newNumber);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException != null && e.InnerException.ToString().Contains("Number"))
                throw new ArgumentException("Esse número da hospedagem já foi cadastrado.");
            else
                throw new Exception();
        }

        return new Response(200, "Número atualizado com sucesso!");
    }
}