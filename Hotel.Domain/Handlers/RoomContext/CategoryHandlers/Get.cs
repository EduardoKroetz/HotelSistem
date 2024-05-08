using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.CategoryHandlers;

public partial class CategoryHandler
{
  public async Task<Response<IEnumerable<GetCategory>>> HandleGetAsync()
  {
    var categories = await _repository.GetAsync();
    return new Response<IEnumerable<GetCategory>>(200,"", categories);
  }
} 