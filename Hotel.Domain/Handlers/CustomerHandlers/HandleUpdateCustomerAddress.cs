using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public new async Task<Response> HandleUpdateAddressAsync(Guid id, ValueObjects.Address address)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var customer = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Usuário não encontrado");

            customer.ChangeAddress(address);

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

            return new Response("Endereço atualizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}