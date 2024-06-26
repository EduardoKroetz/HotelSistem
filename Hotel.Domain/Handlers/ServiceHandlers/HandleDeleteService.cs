using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.ServiceHandler;

public partial class ServiceHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var service = await _repository.GetEntityByIdAsync(id)
              ?? throw new NotFoundException("Serviço não encontrado.");

            _repository.Delete(service);
            await _repository.SaveChangesAsync();
         
            try
            {
                await _stripeService.DisableProductAsync(service.StripeProductId);
            }
            catch(StripeException e)
            {
                throw new StripeException($"Ocorreu um erro ao desativar o produto no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Serviço deletado com sucesso!", new { id });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}