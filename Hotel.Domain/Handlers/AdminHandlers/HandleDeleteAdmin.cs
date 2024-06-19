using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.AdminHandlers;

public partial class AdminHandler
{
    public async Task<Response> HandleDeleteAsync(Guid adminId)
    {
        var admin = await _repository.GetEntityByIdAsync(adminId)
          ?? throw new NotFoundException("Administrador não encontrado.");

        _repository.Delete(admin);
        await _repository.SaveChangesAsync();

        return new Response("Administrador deletado com sucesso!");
    }
}