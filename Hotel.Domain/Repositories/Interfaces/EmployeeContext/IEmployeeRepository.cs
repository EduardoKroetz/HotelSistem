using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>, IRepositoryQuery<GetUser>
{
}