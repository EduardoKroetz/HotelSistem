using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PermissionDTOs;

namespace Hotel.Domain.Handlers.PermissionHandlers;

public partial class PermissionHandler
{
    public async Task<Response<GetPermission>> HandleGetByIdAsync(Guid id)
    {
        var permission = await _repository.GetByIdAsync(id);
        if (permission == null)
            throw new ArgumentException("Permissão não encontrada.");

        return new Response<GetPermission>("Sucesso!", permission);
    }
}