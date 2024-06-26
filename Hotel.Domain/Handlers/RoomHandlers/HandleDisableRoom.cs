using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleDisableRoom(Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _repository.GetRoomIncludesReservations(id)
                ?? throw new NotFoundException("Hospedagem não encontrada.");

            //Validate if the reservation has pending reservations
            var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
            if (pendingReservations.Count > 0)
                throw new InvalidOperationException("Não foi possível desativar a hospedagem pois tem reservas pendentes relacionadas.");

            room.Disable();
            await _repository.SaveChangesAsync();

            try
            {
                await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, false);
            }
            catch
            {
                throw new StripeException("Ocorreu um erro ao atualizar o produto no Stripe.");
            }

            await transaction.CommitAsync();

            return new Response("Hospedagem desativada com sucesso!");
        }
        catch
        {
           await transaction.RollbackAsync();
           throw;
        }
    }
}