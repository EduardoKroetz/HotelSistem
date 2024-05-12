using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.RoomContext;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler : IHandler
{
  private readonly ICategoryRepository  _repository;
  public CategoryHandler(ICategoryRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(EditorCategory model)
  {
    var category = new Category(model.Name,model.Description,model.AveragePrice);

    await _repository.CreateAsync(category);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Categoria criada.",new { category.Id });
  }
}