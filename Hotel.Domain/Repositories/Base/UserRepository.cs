using Hotel.Domain.Data;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Repositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class UserRepository<T> : GenericRepository<T>, IUserRepository<T> where T : User
{
  public UserRepository(HotelDbContext context) : base(context) {}

  public async Task<GetUser?> GetByIdAsync(Guid id)
  {
    return await _context
      .Set<T>()
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetUser(x.Id, x.Name.FirstName,x.Name.LastName, x.Email.Address, x.Phone.Number))
      .FirstOrDefaultAsync();
    

  }
  public async Task<IEnumerable<GetUser>> GetAsync()
  {
    return await _context
      .Set<T>()
      .AsNoTracking()
      .Select(x => new GetUser(x.Id, x.Name.FirstName,x.Name.LastName, x.Email.Address, x.Phone.Number))
      .ToListAsync();
  }
}