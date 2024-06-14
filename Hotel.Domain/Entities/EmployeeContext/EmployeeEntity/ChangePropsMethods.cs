using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee
{
  public void ChangeSalary(decimal? salary)
  {
    if (salary < 0)
      throw new  ValidationException("O sálario do funcionário não pode ser menor ou igual a zero.");
    Salary = salary;
  }
}