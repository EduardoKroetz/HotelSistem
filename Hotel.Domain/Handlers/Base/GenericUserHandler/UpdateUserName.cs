using Hotel.Domain.DTOs;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.Base.GenericUserHandler;

public partial class GenericUserHandler<TRepository, TUser>
{
    public async Task<Response> HandleUpdateNameAsync(Guid userId, Name name)
    {
        var user = await _repository.GetEntityByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Não foi possível encontrar o usuário.");

        user.ChangeName(name);

        await _repository.SaveChangesAsync();

        return new Response(200, "Nome atualizado com sucesso!");
    }
}