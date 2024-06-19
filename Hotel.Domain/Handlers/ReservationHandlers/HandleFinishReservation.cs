using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleFinishReservationAsync(Guid id, Guid customerId)
    {
        var reservation = await _repository.GetReservationIncludesAll(id)
          ?? throw new NotFoundException("Reserva não encontrada.");

        if (reservation.CustomerId != customerId)
            throw new UnauthorizedAccessException("Você não tem permissão para finalizar reserva alheia.");

        var invoice = reservation.Finish(Enums.EPaymentMethod.Pix, 0);

        await _invoiceHandler.HandleCreateAsync(invoice, reservation);

        return new Response("Reserva finalizada com sucesso!");
    }
}