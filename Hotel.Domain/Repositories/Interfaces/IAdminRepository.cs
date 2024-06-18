using Hotel.Domain.DTOs.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IAdminRepository : IRepository<Admin>, IRepositoryQuery<GetUser, GetAdmin, AdminQueryParameters>, IUserRepository<Admin>
{
    Task<Admin?> GetAdminIncludePermissions(Guid adminId);
    Task<Permission?> GetDefaultAdminPermission();
    Task<List<Permission>> GetAllDefaultPermissions();
}