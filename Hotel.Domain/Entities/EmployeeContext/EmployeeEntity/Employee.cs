using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : User
{
  public Employee(Name name, Email email, Phone phone, string password, Responsability responsability, EGender? gender, DateTime? dateOfBirth, Address? address, decimal? salary) 
    : base( name, email, phone, password, gender, dateOfBirth, address)
  {
    Salary = salary;
    Responsabilities = [];
    AddResponsability(responsability);
  }
  
  public decimal? Salary { get; private set; }
  public List<Responsability> Responsabilities { get; private set; } 

  
}