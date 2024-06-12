using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.EmployeeContext;

public class ResponsibilityRepository : GenericRepository<Responsibility>, IResponsibilityRepository
{
  public ResponsibilityRepository(HotelDbContext context) : base(context) { }

  public async Task<GetResponsibility?> GetByIdAsync(Guid id)
  {
    return await _context
      .Responsibilities
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetResponsibility(x.Id, x.Name, x.Description, x.Priority, x.CreatedAt))
      .FirstOrDefaultAsync();

  }
  public async Task<IEnumerable<GetResponsibility>> GetAsync(ResponsibilityQueryParameters queryParameters)
  {
    var query = _context.Responsibilities.AsQueryable();

    if (queryParameters.Name != null)
      query = query.Where(x => x.Name.Contains(queryParameters.Name));

    if (queryParameters.Priority.HasValue)
      query = query.Where(x => x.Priority == queryParameters.Priority);

    if (queryParameters.EmployeeId.HasValue)
      query = query.Where(x => x.Employees.Any(y => y.Id == queryParameters.EmployeeId));

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(y => y.Id == queryParameters.ServiceId));

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetResponsibility
    (
      x.Id,
      x.Name,
      x.Description,
      x.Priority,
      x.CreatedAt
    )).ToListAsync();

  }
}