using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;

public class GetEmployee : GetUser
{
  public GetEmployee(Guid id, string firstName, string lastName ,string email, string phone, EGender? gender, DateTime? dateOfBirth, Address? address, DateTime createdAt,  decimal salary) : base(id,firstName, lastName, email, phone, gender, dateOfBirth, address, createdAt)
  => Salary = salary;

  public decimal Salary { get; private set; }
}
