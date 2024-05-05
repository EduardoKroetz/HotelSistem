using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class ServiceRepository :  GenericRepository<Service> ,IServiceRepository
{
  public ServiceRepository(HotelDbContext context) : base(context) {}
}