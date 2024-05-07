using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;

public partial class RoomInvoiceHandler
{
  public async Task<Response<GetRoomInvoice>> HandleGetByIdAsync(Guid id)
  {
    var roomInvoice = await _repository.GetByIdAsync(id);
    if (roomInvoice == null)
      throw new ArgumentException("Fatura de quarto não encontrada.");
    
    return new Response<GetRoomInvoice>(200,"Fatura de quarto encontrada.", roomInvoice);
  }
}