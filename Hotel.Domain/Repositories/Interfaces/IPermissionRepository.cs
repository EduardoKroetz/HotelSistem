using Hotel.Domain.DTOs.PermissionDTOs;
using Hotel.Domain.Entities.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IPermissionRepository : IRepositoryQuery<GetPermission, PermissionQueryParameters>
{
    Task<Permission?> GetEntityByIdAsync(Guid id);
    Task<IEnumerable<Permission>> GetEntitiesAsync();

}