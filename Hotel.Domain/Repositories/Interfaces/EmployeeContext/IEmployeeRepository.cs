using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces.EmployeeContext;

public interface IEmployeeRepository : IRepository<Employee>, IUserRepository<Employee>, IRepositoryQuery<GetEmployee, EmployeeQueryParameters>
{
  Task<Employee?> GetEmployeeIncludesResponsabilities(Guid id);
  Task<IEnumerable<Permission>> GetAllDefaultPermissions();
  Task<Permission?> GetDefaultPermission();
  Task<Employee?> GetEmployeeIncludesPermissions(Guid id);
}