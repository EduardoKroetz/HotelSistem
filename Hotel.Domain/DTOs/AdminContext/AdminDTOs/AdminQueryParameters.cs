using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.AdminContext.AdminDTOs;
public class AdminQueryParameters : UserQueryParameters
{
  public AdminQueryParameters(int? skip, int? take, string name, string email, string phone, EGender? gender, DateTime? dateOfBirth, DateTime? createdAt, string? createdAtOperator,bool? isRootAdmin, Guid? permissionId) : base(skip, take, name, email, phone, gender, dateOfBirth, createdAt, createdAtOperator)
  {
    IsRootAdmin = isRootAdmin;
    PermissionId = permissionId;
  }

  public bool? IsRootAdmin { get; private set; }
  public Guid? PermissionId { get; set; }
}

