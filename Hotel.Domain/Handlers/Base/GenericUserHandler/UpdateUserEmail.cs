using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Base.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.Base.GenericUserHandler;

public partial class GenericUserHandler<TRepository, TUser>
  where TRepository : IUserRepository<TUser>
  where TUser : User
{
    private readonly TRepository _repository;

    public GenericUserHandler(TRepository repository)
    => _repository = repository;

    public async Task<Response> HandleUpdateEmailAsync(Guid userId, Email email)
    {
        var user = await _repository.GetEntityByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Não foi possível encontrar o usuário.");

        user.ChangeEmail(email);

        await _repository.SaveChangesAsync();

        return new Response(200, "Email atualizado com sucesso!");
    }
}