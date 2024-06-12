using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.Services.Authorization;
using Hotel.Domain.Services.Permissions;
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
        x.Gender,
        x.DateOfBirth,
        x.Address,
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
        x.Gender,
        x.DateOfBirth,
        x.Address,
        x.CreatedAt,
        x.Salary ?? 0
    )).ToListAsync();
  }

  public async Task<Employee?> GetEmployeeIncludesResponsibilities(Guid id)
  {
    return await _context.Employees
      .Where(x => x.Id == id)
      .Include(x => x.Responsibilities)
      .FirstOrDefaultAsync();
  }

  new public async Task<Employee?> GetEntityByEmailAsync(string email)
  {
    return await _context
      .Employees
      .AsNoTracking()
      .Include(x => x.Permissions)
      .Where(x => x.Email.Address == email)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Permission>> GetAllDefaultPermissions()
  {
    var permissions = await _context.Permissions.ToListAsync();

    return permissions
      .Where(p => DefaultEmployeePermissions.PermissionsName
          .Any(pEnum => p.Name.Contains(pEnum.ToString())))
      .ToList();
  }

  public async Task<Permission?> GetDefaultPermission()
  {
    return await _context.Permissions
      .FirstOrDefaultAsync(x => x.Name.Contains("DefaultEmployeePermission"));
  }

  public async Task<Employee?> GetEmployeeIncludesPermissions(Guid id)
  {
    return await _context.Employees
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync(x => x.Id == id);
  }
}