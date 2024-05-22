using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces.AdminContext;

public interface IAdminRepository : IRepository<Admin>, IRepositoryQuery<GetUser, GetAdmin, AdminQueryParameters>, IUserRepository<Admin>
{
  Task<Admin?> GetAdminIncludePermissions(Guid adminId);
}