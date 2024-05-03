using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin : User
{
  public Admin(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
    : base(name,email,phone,password,gender,dateOfBirth,address)
  {
    IsRootAdmin = false;
    Permissions = [];
  }

  public bool IsRootAdmin { get; private set; }
  public HashSet<Permission> Permissions { get; private set; } 

  public void ChangeToRootAdmin(Admin RootAdmin)
  {
    if (RootAdmin.IsRootAdmin)
      IsRootAdmin = true;
    else
      throw new ValidationException("Erro de validação: Não é possível atribuir o status de administrador root a este usuário sem antes possuir um administrador root.");
  }


}