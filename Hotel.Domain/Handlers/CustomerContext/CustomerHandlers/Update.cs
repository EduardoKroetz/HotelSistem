using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler 
{
    public async Task<Response<object>> HandleUpdateAsync(UpdateUser model, Guid id)
  {
    var customer = await _repository.GetEntityByIdAsync(id);
    if (customer == null)
      throw new ArgumentException("Cliente não encontrado.");

    customer.ChangeName(new Name(model.FirstName,model.LastName));
    customer.ChangeEmail(new Email(model.Email));
    customer.ChangePhone(new Phone(model.Phone));
    customer.ChangeGender(model.Gender);
    customer.ChangeDateOfBirth(model.DateOfBirth);
    customer.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));

    _repository.Update(customer);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Cliente atualizado com sucesso!",new { customer.Id });
  }
}