using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler
{
  public async Task<Response<IEnumerable<GetCategory>>> HandleGetAsync(CategoryQueryParameters queryParameters)
  {
    var categories = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetCategory>>(200,"", categories);
  }
} 