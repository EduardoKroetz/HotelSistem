using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Domain.Handlers.ReservationHandlers;

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