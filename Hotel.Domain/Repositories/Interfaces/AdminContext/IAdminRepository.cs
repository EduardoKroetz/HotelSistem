using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces.AdminContext;

public interface IAdminRepository : IRepository<Admin>, IRepositoryQuery<GetUser, GetAdmin, AdminQueryParameters>
{
  Task<Admin?> GetAdminIncludePermissions(Guid adminId);
}