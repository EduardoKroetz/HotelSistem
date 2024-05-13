using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.EmployeeContext;

public class ResponsabilityRepository : GenericRepository<Responsability>, IResponsabilityRepository
{
  public ResponsabilityRepository(HotelDbContext context) : base(context) { }

  public async Task<GetReponsability?> GetByIdAsync(Guid id)
  {
    return await _context
      .Responsabilities
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetReponsability(x.Id, x.Name, x.Description, x.Priority))
      .FirstOrDefaultAsync();

  }
  public async Task<IEnumerable<GetReponsability>> GetAsync(ResponsabilityQueryParameters queryParameters)
  {
    var query = _context.Responsabilities.AsQueryable();

    if (queryParameters.Name != null)
      query = query.Where(x => x.Name.Contains(queryParameters.Name));

    if (queryParameters.Priority.HasValue)
      query = query.Where(x => x.Priority == queryParameters.Priority);

    if (queryParameters.EmployeeId.HasValue)
      query = query.Where(x => x.Employees.Any(y => y.Id == queryParameters.EmployeeId));

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(y => y.Id == queryParameters.ServiceId));

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetReponsability
    (
      x.Id,
      x.Name,
      x.Description,
      x.Priority
    )).ToListAsync();

  }
}