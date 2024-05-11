using Hotel.Domain.Data;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Extensions;
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

  public virtual async Task<List<T>> Query(UserQuery queryParameters)
  {
    var query = _context.Set<T>().AsQueryable();

    if (queryParameters.Name != null)
      query = query.Where(x => x.Name.FirstName.Contains(queryParameters.Name));

    if (queryParameters.Email != null)
      query = query.Where(x => x.Email.Address.Contains(queryParameters.Email));

    if (queryParameters.Phone != null)
      query = query.Where(x => x.Phone.Number.Contains(queryParameters.Phone));

    if (queryParameters.Gender.HasValue)
      query = query.Where(x => x.Gender == queryParameters.Gender);

    if (queryParameters.DateOfBirth.HasValue)
      query = query.Where(x => x.DateOfBirth != null && x.DateOfBirth.Value.Date == queryParameters.DateOfBirth.Value.Date);

    if (queryParameters.CreatedAt.HasValue && queryParameters.CreatedAtOperator != null)
      query = query.FilterCreatedAtOperator(queryParameters.CreatedAtOperator, queryParameters.CreatedAt.Value);

    if (queryParameters.CreatedAtOperator != null)
      query = query.FilterOrderByCreatedAtOperator(queryParameters.CreatedAtOperator);

    var result = await query.ToListAsync();

    return result;
  }
}