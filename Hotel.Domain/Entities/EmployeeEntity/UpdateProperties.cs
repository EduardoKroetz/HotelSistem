using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.EmployeeEntity;

public partial class Employee
{
    public void ChangeSalary(decimal? salary)
    {
        if (salary < 0)
            throw new ValidationException("Salário deve ser maior ou igual a zero");
        Salary = salary;
    }
}