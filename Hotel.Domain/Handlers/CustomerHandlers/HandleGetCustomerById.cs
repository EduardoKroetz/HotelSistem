using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public async Task<Response<GetUser>> HandleGetByIdAsync(Guid id)
    {
        var permission = await _repository.GetByIdAsync(id);
        if (permission == null)
            throw new ArgumentException("Usuário não encontrado.");

        return new Response<GetUser>("Sucesso!", permission);
    }
}