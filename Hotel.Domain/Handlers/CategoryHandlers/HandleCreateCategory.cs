using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.CategoryHandlers;

public partial class CategoryHandler : IHandler
{
    private readonly ICategoryRepository _repository;
    public CategoryHandler(ICategoryRepository repository)
    => _repository = repository;

    public async Task<Response> HandleCreateAsync(EditorCategory model)
    {
        var category = new Category(model.Name, model.Description, model.AveragePrice);

        await _repository.CreateAsync(category);
        await _repository.SaveChangesAsync();

        return new Response(200, "Categoria criada com sucesso!", new { category.Id });
    }
}