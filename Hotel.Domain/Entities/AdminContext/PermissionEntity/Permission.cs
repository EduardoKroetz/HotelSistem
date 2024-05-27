using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission : Entity, IPermission
{
  internal Permission(){}
  public Permission(string name, string description)
  {
    Name = name;
    Description = description;
    IsActive = true;

    Validate();
  }

  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public bool IsActive { get; private set; } = false;
  public ICollection<Admin> Admins { get; private set; } = [];
  public ICollection<Employee> Employees { get; private set; } = [];
}