using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response> HandleUpdateAsync(UpdateUser model, Guid adminId)
  {
    var admin = await _repository.GetEntityByIdAsync(adminId)
      ?? throw new NotFoundException("Administrador não encontrado.");
    
    admin.ChangeName(new Name(model.FirstName,model.LastName));
    admin.ChangePhone(new Phone(model.Phone));
    admin.ChangeGender(model.Gender);
    admin.ChangeDateOfBirth(model.DateOfBirth);
    admin.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));

    try
    {
      _repository.Update(admin);
      await _repository.SaveChangesAsync();
    }
    catch (DbUpdateException e)
    {
      var innerException = e.InnerException?.Message;

      if (innerException != null)
      {

        if (innerException.Contains("Email"))
          return new Response(400, "Esse email já está cadastrado.");

        if (innerException.Contains("Phone"))
          return new Response(400, "Esse telefone já está cadastrado.");
      }
    }


    return new Response(200,"Administrador atualizado com sucesso!");
  }
}