using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces.AdminContext;

public interface IPermissionRepository : IRepositoryQuery<GetPermission, PermissionQueryParameters>
{
  Task<Permission?> GetEntityByIdAsync(Guid id);
  Task<IEnumerable<Permission>> GetEntitiesAsync();
 
}