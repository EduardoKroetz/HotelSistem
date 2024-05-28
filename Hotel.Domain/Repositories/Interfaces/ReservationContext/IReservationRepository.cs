using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;

namespace Hotel.Domain.Repositories.Interfaces.ReservationContext;

public interface IReservationRepository : IRepository<Reservation>, IRepositoryQuery<GetReservation, GetReservationCollection, ReservationQueryParameters>
{
  Task<Reservation?> GetReservationIncludeServices(Guid id);
  Task<Reservation?> GetReservationIncludeCustomers(Guid id);
}