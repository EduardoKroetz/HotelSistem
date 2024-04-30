using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities;

public class Employee : User
{
  public Employee(Name name, Email email, Phone phone, string passwordHash, EGender gender, DateTime dateOfBirth, Address address,decimal salary,Guid empRespId,
    EmployeeResponsability? responsability) 
    : base( name, email, phone, passwordHash, gender, dateOfBirth, address)
  {
    Salary = salary;
    EmpRespId = empRespId;
    Responsability = responsability;
  }
  
  public decimal Salary { get; private set; }
  public Guid EmpRespId { get; private set; }
  public EmployeeResponsability? Responsability { get; private set; } 
}