using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class AdminRepository : GenericRepository<Admin> ,IAdminRepository
{
  public AdminRepository(HotelDbContext context) : base(context) {}

  public async Task<GetAdmin?> GetByIdAsync(Guid id)
  {
    return await _context
      .Admins
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetAdmin(x.Id, x.Email.Address, x.Name.FirstName,x.Name.LastName, x.Phone.Number))
      .FirstOrDefaultAsync();
  
  }
}