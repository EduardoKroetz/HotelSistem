using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public async Task<Response> HandleUpdateAsync(UpdateUser model, Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var customer = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Usuário não encontrado");

            customer.ChangeName(new Name(model.FirstName, model.LastName));
            customer.ChangePhone(new Phone(model.Phone));
            customer.ChangeGender(model.Gender);
            customer.ChangeDateOfBirth(model.DateOfBirth);
            customer.ChangeAddress(new ValueObjects.Address(model.Country, model.City, model.Street, model.Number));

            try
            {
                _repository.Update(customer);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                var innerException = e.InnerException?.Message;

                if (innerException != null)
                {
                    if (innerException.Contains("Email"))
                    {
                        _logger.LogError("Erro ao cadastradar cliente pois o email já está cadastrado");
                        throw new ArgumentException("Esse email já está cadastrado.");
                    }

                    if (innerException.Contains("Phone"))
                    {
                        _logger.LogError("Erro ao cadastradar cliente pois o telefone já está cadastrado");
                        throw new ArgumentException("Esse telefone já está cadastrado.");
                    }
                }
                throw;
            }

            try
            {
                var stripeCustomer = await _stripeService.UpdateCustomerAsync(customer.StripeCustomerId, customer.Name, customer.Email, customer.Phone, customer.Address);
            }
            catch (StripeException)
            {
                _logger.LogError("Erro ao atualizar cliente no stripe");
                throw new StripeException("Ocorreu um erro ao atualizar o usuário no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Usuário atualizado com sucesso!", new { customer.Id, customer.StripeCustomerId });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}