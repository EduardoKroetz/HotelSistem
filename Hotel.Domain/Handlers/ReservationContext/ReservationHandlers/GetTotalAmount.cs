using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> GetTotalAmount(DateTime checkIn, DateTime checkOut, decimal dailyRate, string? servicesIdsStr)
  {
    ICollection<Service> services;
    if (string.IsNullOrEmpty(servicesIdsStr))
      services = [];
    else
    {
      var servicesIds = servicesIdsStr!.Split(',').ToList().ConvertAll(x => Guid.Parse(x));
      services = await _serviceRepository.GetServicesByListId(servicesIds);
    }

    var totalAmount = Reservation.TotalAmount(dailyRate, checkIn, checkOut, services);

    return new Response(200, "Sucesso!", new { totalAmount });
  }
}