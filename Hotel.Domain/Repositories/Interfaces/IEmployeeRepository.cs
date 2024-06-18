using Hotel.Domain.DTOs.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>, IUserRepository<Employee>, IRepositoryQuery<GetEmployee, EmployeeQueryParameters>
{
    Task<Employee?> GetEmployeeIncludesResponsibilities(Guid id);
    Task<IEnumerable<Permission>> GetAllDefaultPermissions();
    Task<Permission?> GetDefaultPermission();
    Task<Employee?> GetEmployeeIncludesPermissions(Guid id);
}