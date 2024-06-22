using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ServiceHandler;

public partial class ServiceHandler : IHandler
{
    private readonly IServiceRepository _repository;
    private readonly IResponsibilityRepository _responsibilityRepository;
    private readonly IStripeService _stripeService;
    public ServiceHandler(IServiceRepository repository, IResponsibilityRepository responsibilityRepository, IStripeService stripeService)
    {
        _repository = repository;
        _responsibilityRepository = responsibilityRepository;
        _stripeService = stripeService;
    }


    public async Task<Response> HandleCreateAsync(EditorService model)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var service = new Service(model.Name, model.Description, model.Price, model.Priority, model.TimeInMinutes);

            try
            {
                await _repository.CreateAsync(service);

                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.ToString().Contains("Name"))
                    throw new ArgumentException("Esse nome já está cadastrado.");
                else
                    throw new Exception("Algum erro ocorreu ao salvar no banco de dados.");
            }

            try
            {
                var product = await _stripeService.CreateProductAsync(service.Name, service.Description, service.Price);
                service.StripeProductId = product.Id;
                await _repository.SaveChangesAsync();
            }
            catch (StripeException)
            {
                throw new StripeException("Um erro ocorreu ao criar o serviço no stripe");
            }

            await transaction.CommitAsync();

            return new Response("Serviço criado com sucesso!", new { service.Id, service.StripeProductId });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}