using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Repositories.Interfaces;


namespace Hotel.Domain.Repositories;

public class AdminRepository : UserRepository<Admin> ,IAdminRepository
{
  public AdminRepository(HotelDbContext context) : base(context) {}

}