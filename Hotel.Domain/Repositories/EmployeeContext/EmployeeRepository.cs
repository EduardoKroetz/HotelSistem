using Hotel.Domain.Data;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class EmployeeRepository : UserRepository<Employee> ,IEmployeeRepository
{
  public EmployeeRepository(HotelDbContext context) : base(context) {}

}