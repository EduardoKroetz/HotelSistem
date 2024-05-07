using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;

public partial class RoomInvoiceHandler
{
  public async Task<Response<IEnumerable<GetRoomInvoice>>> HandleGetAsync()
  {
    var roomInvoices = await _repository.GetAsync();
    return new Response<IEnumerable<GetRoomInvoice>>(200,"", roomInvoices);
  }
}