using Hotel.Domain.DTOs;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Handlers.Base.GenericUserHandler;

public partial class GenericUserHandler<TRepository, TUser>
{
    public async Task<Response> HandleUpdateGenderAsync(Guid userId, EGender gender)
    {
        var user = await _repository.GetEntityByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Não foi possível encontrar o usuário.");

        user.ChangeGender(gender);

        await _repository.SaveChangesAsync();

        return new Response("Gênero atualizado com sucesso!");
    }
}