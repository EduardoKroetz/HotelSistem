using Hotel.Domain.DTOs;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.Base.GenericUserHandler;

public partial class GenericUserHandler<TRepository, TUser>
{
  public async Task<Response> HandleUpdateAddressAsync(Guid userId, Address address)
  {
    var user = await _repository.GetEntityByIdAsync(userId);
    if (user == null)
      throw new ArgumentException("Não foi possível encontrar o usuário.");

    user.ChangeAddress(address);

    await _repository.SaveChangesAsync();

    return new Response(200, "Endereço atualizado com sucesso!");
  }
}