using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<object>> HandleUpdateAsync(UpdateUser model, Guid adminId)
  {
    var admin = await _repository.GetEntityByIdAsync(adminId);
    if (admin == null)
      throw new ArgumentException("Administrador não encontrado.");
    
    admin.ChangeName(new Name(model.FirstName,model.LastName));
    admin.ChangePhone(new Phone(model.Phone));
    admin.ChangeGender(model.Gender);
    admin.ChangeDateOfBirth(model.DateOfBirth);
    admin.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));
    
    _repository.Update(admin);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Administrador atualizado com sucesso!");
  }
}