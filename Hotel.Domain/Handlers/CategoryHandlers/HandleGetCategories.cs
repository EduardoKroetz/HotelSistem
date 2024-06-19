using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CategoryDTOs;

namespace Hotel.Domain.Handlers.CategoryHandlers;

public partial class CategoryHandler
{
    public async Task<Response<IEnumerable<GetCategory>>> HandleGetAsync(CategoryQueryParameters queryParameters)
    {
        var categories = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetCategory>>("Sucesso!", categories);
    }
}