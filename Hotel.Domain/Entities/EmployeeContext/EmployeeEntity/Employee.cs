using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : User, IEmployee
{
  private Employee(){}

  public Employee(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null, decimal? salary = null) 
    : base( name, email, phone, password, gender, dateOfBirth, address)
  {
    Salary = salary;
  }
  
  public decimal? Salary { get; private set; }
  public HashSet<Responsability> Responsabilities { get; private set; } = [];

}