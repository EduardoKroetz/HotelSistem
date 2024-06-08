using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler 
{
  public async Task<Response> HandleUpdateAsync(UpdateUser model, Guid id)
  {
    var customer = await _repository.GetEntityByIdAsync(id);
    if (customer == null)
      throw new ArgumentException("Usuário não encontrado.");

    customer.ChangeName(new Name(model.FirstName,model.LastName));
    customer.ChangePhone(new Phone(model.Phone));
    customer.ChangeGender(model.Gender);
    customer.ChangeDateOfBirth(model.DateOfBirth);
    customer.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));

    _repository.Update(customer);
    await _repository.SaveChangesAsync();

    return new Response(200,"Usuário atualizado com sucesso!",new { customer.Id });
  }
}