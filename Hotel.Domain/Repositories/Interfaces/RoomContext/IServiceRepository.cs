using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface IServiceRepository : IRepository<Service>, IRepositoryQuery<GetService, GetServiceCollection, ServiceQueryParameters>
{
  public Task<Service?> GetServiceIncludeResponsabilities(Guid serviceId);
  Task<ICollection<Service>> GetServicesByListId(ICollection<Guid> servicesIds);
}