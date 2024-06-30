using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Base.Interfaces;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.Base.GenericUserHandler;

public partial class GenericUserHandler<TRepository, TUser>
  where TRepository : IUserRepository<TUser>
  where TUser : User
{
    private readonly TRepository _repository;
    private readonly IEmailService _emailService;

    public GenericUserHandler(TRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<Response> HandleUpdateEmailAsync(Guid userId, Email email, string? code)
    {
        //Validação do código
        await _emailService.VerifyEmailCodeAsync(email, code);

        var user = await _repository.GetEntityByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Não foi possível encontrar o usuário.");

        user.ChangeEmail(email);

        await _repository.SaveChangesAsync();

        return new Response("Email atualizado com sucesso!");
    }
}