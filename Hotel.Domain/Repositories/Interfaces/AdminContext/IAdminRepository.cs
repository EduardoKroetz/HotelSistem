using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IAdminRepository : IRepository<Admin>, IRepositoryQuery<GetUser>
{
}