using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AuthenticationContext.RegisterHandlers;

public partial class RegisterHandler
{
  public async Task<Response<Guid>> HandleRegisterAdmin(CreateUser model)
  {
    var admin = new Admin(
      new Name(model.FirstName, model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password, //Vai hashear a senha no construtor
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country, model.City, model.Street, model.Number)
    );

    await _adminRepository.CreateAsync(admin);
    await _adminRepository.SaveChangesAsync();

    return new Response<Guid>(200, "Administrador criado com sucesso!", admin.Id);
  }
}