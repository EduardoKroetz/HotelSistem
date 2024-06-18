using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IServiceRepository : IRepository<Service>, IRepositoryQuery<GetService, ServiceQueryParameters>
{
    public Task<Service?> GetServiceIncludeResponsibilities(Guid serviceId);
    Task<ICollection<Service>> GetServicesByListId(ICollection<Guid> servicesIds);
}