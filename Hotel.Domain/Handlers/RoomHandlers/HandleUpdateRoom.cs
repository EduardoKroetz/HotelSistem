using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateAsync(EditorRoom model, Guid id)
    {
        var room = await _repository.GetRoomIncludesReservations(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
        if (pendingReservations.Count > 0 && model.Price != room.Price)
            throw new InvalidOperationException("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas a hospedagem.");

        var hasUniqueName = await _repository.GetRoomByName(model.Name);
        if (hasUniqueName != null)
            throw new ArgumentException("Esse nome já foi cadastrado.");

        var hasUniqueNumber = await _repository.GetRoomByNumber(model.Number);
        if (hasUniqueNumber != null)
            throw new ArgumentException("Esse número já foi cadastrado.");

        room.ChangeName(model.Name);
        room.ChangeNumber(model.Number);
        room.ChangeCapacity(model.Capacity);
        room.ChangePrice(model.Price);
        room.ChangeDescription(model.Description);
        room.ChangeCategory(model.CategoryId);

        await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, room.IsActive);

    
        _repository.Update(room);
        await _repository.SaveChangesAsync();

        return new Response("Hospedagem atualizada com sucesso!", new { room.Id });
    }
}