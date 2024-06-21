using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        var room = await _repository.GetRoomIncludesReservations(id)
          ?? throw new NotFoundException("Hospedagem n�o encontrada.");

        if (room.Reservations.Count > 0)
            throw new InvalidOperationException("N�o foi poss�vel deletar a hospedagem pois tem reservas associadas a ela. Sugiro que desative a hospedagem.");

        //Delete room in Stripe
        if (await _stripeService.DisableProductAsync(room.StripeProductId) == null)
            throw new Exception("N�o foi poss�vel desabilitar o produto no servi�o do Stripe.");    

        _repository.Delete(room);
        await _repository.SaveChangesAsync();
        return new Response("Hospedagem deletada com sucesso!", new { id });
    }
}