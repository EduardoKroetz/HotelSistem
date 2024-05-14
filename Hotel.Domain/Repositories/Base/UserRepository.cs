using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.Base;

public abstract class UserRepository<T> : GenericRepository<T>, IUserRepository<T> where T : User
{
  public UserRepository(HotelDbContext context) : base(context) { }

  public async Task<GetUser?> GetByIdAsync(Guid id)
  {
    return await _context
      .Set<T>()
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(c => new GetUser(c.Id, c.Name.FirstName, c.Name.LastName, c.Email.Address, c.Phone.Number, c.Gender, c.DateOfBirth, c.Address, c.CreatedAt))
      .FirstOrDefaultAsync();


  }

  public virtual IQueryable<T> GetAsync(UserQueryParameters queryParameters)
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
      query = query.FilterByOperator(queryParameters.DateOfBirthOperator,x => x.DateOfBirth,queryParameters.DateOfBirth);

    query = query.BaseQuery(queryParameters);

    return query;
  }

}