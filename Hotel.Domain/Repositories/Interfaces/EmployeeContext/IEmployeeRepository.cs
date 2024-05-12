using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

namespace Hotel.Domain.Repositories.Interfaces.EmployeeContext;

public interface IEmployeeRepository : IRepository<Employee>, IRepositoryQuery<GetEmployee, EmployeeQueryParameters>
{
}