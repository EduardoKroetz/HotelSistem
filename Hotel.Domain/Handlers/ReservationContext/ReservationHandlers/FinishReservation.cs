using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleFinishReservationAsync(Guid id)
  {
    //temporário
    var reservation = await _repository.GetReservationIncludesAll(id);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");

    var invoice = reservation.GenerateInvoice(Enums.EPaymentMethod.Pix, 0);

    await _invoiceHandler.HandleCreateAsync(invoice, reservation);

    return new Response(200, "Sucesso!");
  }
}