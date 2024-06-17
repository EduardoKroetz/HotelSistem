using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface IServiceRepository : IRepository<Service>, IRepositoryQuery<GetService, ServiceQueryParameters>
{
  public Task<Service?> GetServiceIncludeResponsibilities(Guid serviceId);
  Task<ICollection<Service>> GetServicesByListId(ICollection<Guid> servicesIds);
}