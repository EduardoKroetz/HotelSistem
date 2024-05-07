using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler 
{
  public async Task<Response<GetRoom>> HandleGetByIdAsync(Guid id)
  {
    var room = await _repository.GetByIdAsync(id);
    if (room == null)
      throw new ArgumentException("Hospedagem não encontrada.");
    
    return new Response<GetRoom>(200,"Hospedagem encontrada.", room);
  }
}