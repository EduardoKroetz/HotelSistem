using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.Interfaces;
using System.Data.Common;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler : IHandler
{
    private readonly IRoomRepository _repository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStripeService _stripeService;

    public RoomHandler(IRoomRepository repository, IServiceRepository serviceRepository, ICategoryRepository categoryRepository, IStripeService stripeService)
    {
        _repository = repository;
        _serviceRepository = serviceRepository;
        _categoryRepository = categoryRepository;
        _stripeService = stripeService;
    }

    public async Task<Response> HandleCreateAsync(EditorRoom model)
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
            
        var stripeProduct = await _stripeService.CreateProductAsync(model.Name, model.Description, model.Price);

        var room = new Room(model.Name ,model.Number, model.Price, model.Capacity, model.Description, category, stripeProduct.Id);
  
        try
        {
            await _repository.CreateAsync(room);
            await _repository.SaveChangesAsync();
        }catch (DbException)
        {
            await _stripeService.DisableProductAsync(stripeProduct.Id);
            throw;
        }

     
        return new Response("Hospedagem criada com sucesso!", new { room.Id });
    }
}