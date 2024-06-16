using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.RoomContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler : IHandler
{
  private readonly IRoomRepository  _repository;
  private readonly IServiceRepository _serviceRepository;
  private readonly ICategoryRepository _categoryRepository;

  public RoomHandler(IRoomRepository repository, IServiceRepository serviceRepository, ICategoryRepository categoryRepository)
  {
    _repository = repository;
    _serviceRepository = serviceRepository;
    _categoryRepository = categoryRepository;
  }


  public async Task<Response> HandleCreateAsync(EditorRoom model)
  {
    var category = await _categoryRepository.GetEntityByIdAsync(model.CategoryId);
    if (category == null)
      throw new NotFoundException("Categoria não encontrada.");

    var room = new Room(model.Number,model.Price,model.Capacity,model.Description,model.CategoryId);

    try
    {
      await _repository.CreateAsync(room);
      await _repository.SaveChangesAsync();
    }
    catch (DbUpdateException e)
    {
      if (e.InnerException != null && e.InnerException.ToString().Contains("Number"))
        throw new ArgumentException("Esse número da hospedagem já foi cadastrado.");
      else
        throw new Exception();
    }


    return new Response(200,"Hospedagem criada com sucesso!",new { room.Id });
  }
}