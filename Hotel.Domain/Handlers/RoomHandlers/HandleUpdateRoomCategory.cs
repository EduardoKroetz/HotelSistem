using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateCategoryAsync(Guid id, Guid categoryId)
    {
        var room = await _repository.GetEntityByIdAsync(id)
         ?? throw new NotFoundException("Hospedagem não encontrada.");

        var category = await _categoryRepository.GetEntityByIdAsync(categoryId)
          ?? throw new NotFoundException("Categoria não encontrada.");

        room.ChangeCategory(categoryId);

        await _repository.SaveChangesAsync();

        return new Response(200, "Categoria atualizada com sucesso!");
    }
}