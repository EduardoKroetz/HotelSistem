using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response<object>> HandleUpdateCategoryAsync(Guid id, Guid categoryId)
  {
    var room = await _repository.GetEntityByIdAsync(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");

    var category = await _categoryRepository.GetEntityByIdAsync(categoryId);
    if (category == null)
      throw new ArgumentException("Categoria não encontrada.");

    room.ChangeCategory(categoryId);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Categoria atualizada.");
  }
}