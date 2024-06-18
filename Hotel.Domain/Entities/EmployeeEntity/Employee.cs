using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.EmployeeEntity;

public partial class Employee : User, IEmployee
{
    internal Employee() { }

    public Employee(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null, decimal? salary = null, ICollection<Permission>? permissions = null)
      : base(name, email, phone, password, gender, dateOfBirth, address)
    {
        Salary = salary;
        Permissions = permissions ?? [];
    }

    public decimal? Salary { get; private set; }
    public ICollection<Responsibility> Responsibilities { get; private set; } = [];
    public ICollection<Report> Reports { get; private set; } = [];
    public ICollection<Permission> Permissions { get; private set; } = [];
}