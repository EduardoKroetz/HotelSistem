using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler 
{
    public async Task<Response<object>> HandleUpdateAsync(EditorCategory model, Guid id)
  {
    var category = await _repository.GetEntityByIdAsync(id);
    if (category == null)
      throw new ArgumentException("Categoria não encontrada.");

    category.ChangeName(model.Name);
    category.ChangeDescription(model.Description);
    category.ChangeAveragePrice(model.AveragePrice);

    _repository.Update(category);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Categoria foi atualizada.",new { category.Id });
  }
}