using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;

public partial class RoomInvoiceHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Fatura deletada.", new { id });
  }
}