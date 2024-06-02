using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler
{
  public async Task<Response<GetCategory>> HandleGetByIdAsync(Guid id)
  {
    var category = await _repository.GetByIdAsync(id);
    if (category == null)
      throw new ArgumentException("Categoria não encontrada.");
    
    return new Response<GetCategory>(200, "Sucesso!", category);
  }
}