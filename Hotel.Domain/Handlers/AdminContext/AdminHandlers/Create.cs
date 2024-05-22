
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler : GenericUserHandler<IAdminRepository,Admin>, IHandler
{
  private readonly IAdminRepository _repository;
  private readonly IPermissionRepository _permissionRepository; 
  public AdminHandler(IAdminRepository repository, IPermissionRepository permissionRepository) : base(repository)
  {
    _repository = repository;
    _permissionRepository = permissionRepository;
  }

  public async Task<Response<object>> HandleCreateAsync(CreateUser model)
  {
    var admin = new Admin(
      new Name(model.FirstName,model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number)
    );

    await _repository.CreateAsync(admin);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Administrador criado.",new { admin.Id });
  }
}