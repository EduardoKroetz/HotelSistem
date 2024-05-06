
using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler : IHandler
{
  private readonly IAdminRepository _repository;
  public AdminHandler(IAdminRepository repository)
  => _repository = repository;

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

    return new Response<object>(200,"Admin criado com sucesso!",new { admin.Id });
  }
}