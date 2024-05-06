using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler : IHandler
{
  private readonly ICustomerRepository  _repository;
  public CustomerHandler(ICustomerRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(CreateUser model)
  {
    var customer = new Customer(
      new Name(model.FirstName,model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number)
    );

    await _repository.CreateAsync(customer);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Cliente criado com sucesso!",new { customer.Id });
  }
}