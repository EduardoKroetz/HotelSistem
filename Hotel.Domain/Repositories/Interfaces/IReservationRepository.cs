using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Entities.ReservationEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IReservationRepository : IRepository<Reservation>, IRepositoryQuery<GetReservation, ReservationQueryParameters>
{
    Task<Reservation?> GetReservationIncludesServices(Guid id);
    Task<Reservation?> GetReservationIncludesCustomer(Guid id);
    Task<Reservation?> GetReservationIncludesAll(Guid id);
}