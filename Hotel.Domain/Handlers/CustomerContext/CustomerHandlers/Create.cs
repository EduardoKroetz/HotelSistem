using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Services.EmailServices.Interface;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler : GenericUserHandler<ICustomerRepository,Customer>, IHandler
{
  private readonly ICustomerRepository  _repository;
  private readonly IEmailService _emailService;

  public CustomerHandler(ICustomerRepository repository, IEmailService emailService) : base(repository)
  {
    _repository = repository;
    _emailService = emailService;
  }

  public async Task<Response> HandleCreateAsync(CreateUser model, string? code)
  {
    var email = new Email(model.Email);
    await _emailService.VerifyEmailCodeAsync(email,code);

    var customer = new Customer(
      new Name(model.FirstName,model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number)
    );


    try
    {
      await _repository.CreateAsync(customer);
      await _repository.SaveChangesAsync();
    }
    catch (DbUpdateException e)
    {
      var innerException = e.InnerException?.Message;

      if (innerException != null)
      {

        if (innerException.Contains("Email"))
          throw new ArgumentException("Esse email já está cadastrado.");

        if (innerException.Contains("Phone"))
          throw new ArgumentException("Esse telefone já está cadastrado.");
      }
    }

    return new Response(200,"Cadastro realizado com sucesso!",new { customer.Id });
  }
}