using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> GetTotalAmount(DateTime checkIn, DateTime checkOut, decimal dailyRate, ICollection<Guid> servicesIds)
  {
    var services = await _serviceRepository.GetServicesByListId(servicesIds);

    var totalAmount = Reservation.TotalAmount(dailyRate, checkIn, checkOut, services);

    return new Response(200, "Sucesso!", new { totalAmount });
  }
}