using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.CustomerHandlers;

public partial class CustomerHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var customer = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Usuário não encontrado");

            try
            {
                _repository.Delete(customer);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao deletar o usuário do banco de dados.");
            }

            try
            {
                await _stripeService.DeleteCustomerAsync(customer.StripeCustomerId);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao deletar o usuário do banco de dados.");
            }

            await transaction.CommitAsync();

            return new Response("Usuário deletado com sucesso!", new { customer.Id , customer.StripeCustomerId });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}