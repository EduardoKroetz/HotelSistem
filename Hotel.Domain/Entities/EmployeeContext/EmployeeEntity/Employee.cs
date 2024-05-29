using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : User, IEmployee
{
  internal Employee(){}

  public Employee(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null, decimal? salary = null,ICollection<Permission>? permissions = null) 
    : base( name, email, phone, password, gender, dateOfBirth, address)
  {
    Salary = salary;
    Permissions = permissions ?? [];
  }
  
  public decimal? Salary { get; private set; }
  public HashSet<Responsability> Responsabilities { get; private set; } = [];
  public HashSet<Report> Reports { get; private set; } = [];
  public ICollection<Permission> Permissions { get; private set; } = [];
}