using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Stripe;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public new async Task<Response> HandleUpdateEmailAsync(Guid id, Email newEmail, string code)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            await _emailService.VerifyEmailCodeAsync(newEmail, code);

            var customer = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Usuário não encontrado");

            customer.ChangeEmail(newEmail);

            await _repository.SaveChangesAsync();

            try
            {
                await _stripeService.UpdateCustomerAsync(customer.StripeCustomerId, customer.Name, customer.Email, customer.Phone, customer.Address);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao atualizar o cliente no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Email atualizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}