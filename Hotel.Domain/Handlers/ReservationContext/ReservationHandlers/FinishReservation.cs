using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleFinishReservationAsync(Guid id)
  {
    var reservation = await _repository.GetReservationIncludesAll(id)
    ?? throw new NotFoundException("Reserva não encontrada.");

    var invoice = reservation.GenerateInvoice(Enums.EPaymentMethod.Pix, 0);

    await _invoiceHandler.HandleCreateAsync(invoice, reservation);

    return new Response(200, "Sucesso!");
  }
}