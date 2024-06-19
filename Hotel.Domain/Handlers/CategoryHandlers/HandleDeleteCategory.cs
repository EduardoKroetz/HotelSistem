using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.CategoryHandlers;

public partial class CategoryHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        var category = await _repository.GetCategoryIncludesRooms(id);
        if (category == null)
            throw new NotFoundException("Categoria não encontrada.");

        if (category.Rooms.Count > 0)
            throw new InvalidOperationException("Não é possível deletar a categoria pois tem hospedagems associados a ela. Sugiro que troque a categoria dos quartos associados.");

        _repository.Delete(category);
        await _repository.SaveChangesAsync();
        return new Response("Categoria deletada com sucesso!", new { id });
    }
}