using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin : User, IAdmin
{
  internal Admin(){}

  public Admin(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null, List<Permission>? defaultPermissions = null)
    : base(name,email,phone,password,gender,dateOfBirth,address) 
  {
    Permissions = defaultPermissions ?? [];
  }

  public bool IsRootAdmin { get; private set; } = false;
  public ICollection<Permission> Permissions { get; private set; } = [];

  public void ChangeToRootAdmin(Admin RootAdmin)
  {
    if (IsRootAdmin)
      throw new InvalidOperationException("Esse administrador já é um administrador raiz.");
    if (RootAdmin.IsRootAdmin)
      IsRootAdmin = true;
    else
      throw new ValidationException("Esse administrador não é um administrador raiz. Informe um administrador raiz para mudar o status.");
  }


}