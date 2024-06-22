using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateAsync(EditorRoom model, Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var category = await _categoryRepository.GetEntityByIdAsync(model.CategoryId)
                ?? throw new NotFoundException("Categoria não encontrada.");

            //Get room with all associated reservations
            var room = await _repository.GetRoomIncludesReservations(id)
                ?? throw new NotFoundException("Hospedagem não encontrada.");

            //Validate if has associated pending reservations
            var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
            if (pendingReservations.Count > 0 && model.Price != room.Price)
                throw new InvalidOperationException("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas a hospedagem.");

            var hasUniqueName = await _repository.GetRoomByName(model.Name);
            if (hasUniqueName != null && hasUniqueName.Id != room.Id)
                throw new ArgumentException("Esse nome já foi cadastrado.");

            var hasUniqueNumber = await _repository.GetRoomByNumber(model.Number);
            if (hasUniqueNumber != null && hasUniqueNumber.Id != room.Id)
                throw new ArgumentException("Esse número já foi cadastrado.");

            room.ChangeName(model.Name);
            room.ChangeNumber(model.Number);
            room.ChangeCapacity(model.Capacity);
            room.ChangePrice(model.Price);
            room.ChangeDescription(model.Description);
            room.ChangeCategory(model.CategoryId);

            await _repository.SaveChangesAsync();

            try
            {
                await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, room.IsActive);
            }
            catch
            {
                throw new StripeException("Um erro ocorreu ao atualizar o produto no Stripe.");
            }

            await transaction.CommitAsync();

            return new Response("Hospedagem atualizada com sucesso!", new { room.Id });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}