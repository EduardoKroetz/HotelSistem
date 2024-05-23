﻿using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response<object>> HandleRemoveServiceAsync(Guid id, Guid serviceId)
  {
    //Somente admins tem acesso
    var reservation = await _repository.GetReservationIncludeServices(id);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");

    var service = await _serviceRepository.GetEntityByIdAsync(serviceId);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    reservation.RemoveService(service);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Serviço removido.");
  }
}