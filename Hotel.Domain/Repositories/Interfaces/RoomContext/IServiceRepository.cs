using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IServiceRepository : IRepository<Service>, IRepositoryQuery<GetService,GetServiceCollection>
{
}