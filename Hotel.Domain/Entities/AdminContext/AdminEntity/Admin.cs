using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin : User
{
  public Admin(Name name, Email email, Phone phone, string password, EGender? gender, DateTime? dateOfBirth, Address? address) 
    : base(name,email,phone,password,gender,dateOfBirth,address)
  {
    IsRootAdmin = false;
    Permissions = [];
  }

  public bool IsRootAdmin { get; private set; }
  public List<Permission> Permissions { get; private set; } 

  public void ChangeToRootAdmin(Admin RootAdmin)
  {
    if (RootAdmin.IsRootAdmin)
      IsRootAdmin = true;
  }


}