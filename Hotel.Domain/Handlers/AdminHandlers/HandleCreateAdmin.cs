
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.AdminHandlers;

public partial class AdminHandler : GenericUserHandler<IAdminRepository, Admin>, IHandler
{
    private readonly IAdminRepository _repository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IEmailService _emailService;

    public AdminHandler(IAdminRepository repository, IPermissionRepository permissionRepository, IEmailService emailService) : base(repository)
    {
        _repository = repository;
        _permissionRepository = permissionRepository;
        _emailService = emailService;
    }

    public async Task<Response> HandleCreateAsync(CreateUser model, string? code)
    {
        //Validação do código
        var email = new Email(model.Email);
        await _emailService.VerifyEmailCodeAsync(email, code);

        //Criação do administrador

        var defaultAdminPermission = await _repository.GetDefaultAdminPermission() ?? throw new NotFoundException("Permissão padrão não encontrada.");

        var admin = new Admin(
          new Name(model.FirstName, model.LastName),
          email,
          new Phone(model.Phone),
          model.Password,
          model.Gender,
          model.DateOfBirth,
          new Address(model.Country, model.City, model.Street, model.Number),
          [defaultAdminPermission]
        );

        try
        {
            await _repository.CreateAsync(admin);

            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException?.Message;

            if (innerException != null)
            {

                if (innerException.Contains("Email"))
                    throw new ArgumentException("Esse email já está cadastrado.");

                if (innerException.Contains("Phone"))
                    throw new ArgumentException("Esse telefone já está cadastrado.");
            }
        }


        return new Response("Administrador criado com sucesso!", new { admin.Id });
    }
}