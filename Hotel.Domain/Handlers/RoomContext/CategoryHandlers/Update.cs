using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler 
{
  public async Task<Response> HandleUpdateAsync(EditorCategory model, Guid id)
  {
    var category = await _repository.GetEntityByIdAsync(id);
    if (category == null)
      throw new NotFoundException("Categoria não encontrada.");

    category.ChangeName(model.Name);
    category.ChangeDescription(model.Description);
    category.ChangeAveragePrice(model.AveragePrice);

    _repository.Update(category);
    await _repository.SaveChangesAsync();

    return new Response(200,"Categoria atualizada com sucesso!",new { category.Id });
  }
}