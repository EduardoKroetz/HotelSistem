using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IPermissionRepository : IRepository<Permission>, IRepositoryQuery<GetPermission,PermissionQueryParameters>
{
}