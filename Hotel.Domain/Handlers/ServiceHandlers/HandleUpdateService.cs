using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ServiceHandler;

public partial class ServiceHandler
{
    public async Task<Response> HandleUpdateAsync(EditorService model, Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var service = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Serviço não encontrado.");

            service.ChangeName(model.Name);
            service.ChangeDescription(model.Description);
            service.ChangePriority(model.Priority);
            service.ChangeTime(model.TimeInMinutes);
            service.ChangePrice(model.Price);

            try
            {
                _repository.Update(service);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.ToString().Contains("Name"))
                    throw new ArgumentException("Esse nome já está cadastrado.");
                throw;
            }

            try
            {
                await _stripeService.UpdateProductAsync(service.StripeProductId, service.Name, service.Description, service.Price, service.IsActive);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao atualizar o produto no Stripe");
            }


            await transaction.CommitAsync();

            return new Response("Serviço atualizado com sucesso!", new { service.Id });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}