using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler : GenericUserHandler<ICustomerRepository, Hotel.Domain.Entities.CustomerEntity.Customer>,IHandler
{
    private readonly ICustomerRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IStripeService _stripeService;

    public CustomerHandler(ICustomerRepository repository, IEmailService emailService, IStripeService stripeService) : base(repository, emailService)
    {
        _repository = repository;
        _emailService = emailService;
        _stripeService = stripeService;
    }

    public async Task<Response> HandleCreateAsync(CreateUser model, string? code)
    {
        var email = new Email(model.Email);
        await _emailService.VerifyEmailCodeAsync(email, code);

        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var name = new Name(model.FirstName, model.LastName);

            var customer = new Entities.CustomerEntity.Customer
            (
                name,
                email,
                new Phone(model.Phone),
                model.Password,
                model.Gender,
                model.DateOfBirth,
                new ValueObjects.Address(model.Country, model.City, model.Street, model.Number)
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
                        throw new ArgumentException("Esse email j� est� cadastrado.");

                    if (innerException.Contains("Phone"))
                        throw new ArgumentException("Esse telefone j� est� cadastrado.");
                }
                throw new DbUpdateException("Ocorreu um erro ao criar o cliente no banco de dados");
            }

            try
            {
                var stripeCustomer = await _stripeService.CreateCustomerAsync(customer.Name, customer.Email, customer.Phone, customer.Address);
                customer.StripeCustomerId = stripeCustomer.Id;
                await _repository.SaveChangesAsync();

            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao criar o cliente no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Cadastro realizado com sucesso!", new { customer.Id, customer.StripeCustomerId });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }  
    }
}