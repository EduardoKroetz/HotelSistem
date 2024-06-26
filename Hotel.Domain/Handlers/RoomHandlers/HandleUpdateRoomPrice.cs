using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdatePriceAsync(Guid id, decimal price)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _repository.GetRoomIncludesReservations(id)
                ?? throw new NotFoundException("Hospedagem não encontrada.");

            var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
            if (pendingReservations.Count > 0 && price != room.Price)
                throw new InvalidOperationException("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas a hospedagem.");

            room.ChangePrice(price);
            await _repository.SaveChangesAsync();


            try
            {
                await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price);
            }
            catch
            {
                throw new StripeException("Um erro ocorreu ao atualizar o produto no Stripe.");
            }

            await transaction.CommitAsync();
            return new Response("Preço atualizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

       
    }
}