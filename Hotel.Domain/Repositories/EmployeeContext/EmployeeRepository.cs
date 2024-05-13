using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.EmployeeContext;

public class EmployeeRepository : UserRepository<Employee>, IEmployeeRepository
{
  public EmployeeRepository(HotelDbContext context) : base(context) { }

  public new async Task<GetEmployee?> GetByIdAsync(Guid id)
  {
    return await _context
      .Employees
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetEmployee
      (
        x.Id,
        x.Name.FirstName,
        x.Name.LastName,
        x.Email.Address,
        x.Phone.Number,
        x.CreatedAt,
        x.Salary ?? 0
      ))
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<GetEmployee>> GetAsync(EmployeeQueryParameters queryParameters)
  {
    var query = base.GetAsync(queryParameters);

    if (queryParameters.Salary.HasValue)
      query = query.FilterByOperator(queryParameters.SalaryOperator, x => x.Salary , queryParameters.Salary);

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetEmployee
    (
        x.Id,
        x.Name.FirstName,
        x.Name.LastName,
        x.Email.Address,
        x.Phone.Number,
        x.CreatedAt,
        x.Salary ?? 0
    )).ToListAsync();
  }
}