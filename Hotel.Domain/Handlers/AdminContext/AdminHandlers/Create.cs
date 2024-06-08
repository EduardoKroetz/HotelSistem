
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Services.EmailServices.Interface;
using Hotel.Domain.Services.Permissions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler : GenericUserHandler<IAdminRepository,Admin>, IHandler
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
    var response = await _emailService.VerifyEmailCodeAsync(email, code);
    if (response.Status != 200)
      return response;

    //Criação do administrador

    var defaultAdminPermission = DefaultAdminPermissions.DefaultPermission ?? await _repository.GetDefaultAdminPermission() ?? throw new NotFoundException("Permissão padrão não encontrada.");

    var admin = new Admin(
      new Name(model.FirstName,model.LastName),
      email,
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number),
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
          return new Response(400,"Esse email já está cadastrado.");

        if (innerException.Contains("Phone"))
          return new Response(400,"Esse telefone já está cadastrado.");
      }
    }
 

    return new Response(200,"Administrador criado com sucesso!",new { admin.Id });
  }
}