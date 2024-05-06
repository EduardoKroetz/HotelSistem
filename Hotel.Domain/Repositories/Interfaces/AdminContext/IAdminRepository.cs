using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IAdminRepository : IRepository<Admin>, IRepositoryQuery<GetAdmin>
{
}