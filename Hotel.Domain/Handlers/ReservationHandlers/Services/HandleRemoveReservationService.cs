using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleRemoveServiceAsync(Guid id, Guid serviceId)
    {
        //Somente admins tem acesso
        var reservation = await _repository.GetReservationIncludesServices(id)
          ?? throw new NotFoundException("Reserva não encontrada.");

        var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
          ?? throw new NotFoundException("Serviço não encontrado.");

        reservation.RemoveService(service);

        await _repository.SaveChangesAsync();

        return new Response("Serviço removido com sucesso!");
    }
}