using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Stripe;


namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler : IHandler
{
    private readonly IRoomRepository _repository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStripeService _stripeService;
    private readonly ILogger<RoomHandler> _logger;

    public RoomHandler(IRoomRepository repository, IServiceRepository serviceRepository, ICategoryRepository categoryRepository, IStripeService stripeService, ILogger<RoomHandler> logger)
    {
        _repository = repository;
        _serviceRepository = serviceRepository;
        _categoryRepository = categoryRepository;
        _stripeService = stripeService;
        _logger = logger;
    }

    public async Task<Response> HandleCreateAsync(EditorRoom model)
    {
        var transaction = await _repository.BeginTransactionAsync();
        
        try
        {
            var category = await _categoryRepository.GetEntityByIdAsync(model.CategoryId);
            if (category == null)
                throw new NotFoundException("Categoria não encontrada.");

            var hasUniqueName = await _repository.GetRoomByName(model.Name);
            if (hasUniqueName != null)
                throw new ArgumentException("Esse nome já foi cadastrado.");

            var hasUniqueNumber = await _repository.GetRoomByNumber(model.Number);
            if (hasUniqueNumber != null)
                throw new ArgumentException("Esse número já foi cadastrado.");

            var room = new Room(model.Name, model.Number, model.Price, model.Capacity, model.Description, category);

            try
            {
                await _repository.CreateAsync(room);
                await _repository.SaveChangesAsync();
            }catch (Exception e)
            {
                _logger.LogError($"Erro ao criar cômodo no banco de dados. Erro: {e.Message}");
            }

            try
            {
                var stripeProduct = await _stripeService.CreateProductAsync(model.Name, model.Description, model.Price);
                room.StripeProductId = stripeProduct.Id;
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao criar cômodo como produto no Stripe. Erro: {e.Message}");
                throw new StripeException("Um ero ocorreu ao criar o produto no Stripe.");
            }

            await transaction.CommitAsync();

            return new Response("Hospedagem criada com sucesso!", new { room.Id });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}