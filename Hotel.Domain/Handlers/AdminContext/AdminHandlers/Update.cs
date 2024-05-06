using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<object>> HandleUpdateAsync(UpdateAdmin model, Guid adminId)
  {
    var admin = await _repository.GetEntityByIdAsync(adminId);
    if (admin == null)
      throw new ArgumentException("Não foi possível atualizar pois o Admin não existe.");
    
    admin.ChangeName(new Name(model.FirstName,model.LastName));
    admin.ChangeEmail(new Email(model.Email));
    admin.ChangePhone(new Phone(model.Phone));
    admin.ChangeGender(model.Gender);
    admin.ChangeDateOfBirth(model.DateOfBirth);
    admin.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));
    
    _repository.Update(admin);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Admin atualizado com sucesso!",new { admin.Id });
  }
}