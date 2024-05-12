using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;

public class GetEmployee : GetUser
{
  public GetEmployee(Guid id, string firstName, string lastName ,string email, string phone, DateTime createdAt,  decimal salary) : base(id,firstName, lastName, email, phone, createdAt)
  => Salary = salary;


  public decimal Salary { get; set; }
}
