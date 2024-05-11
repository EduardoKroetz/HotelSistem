using Hotel.Domain.DTOs.User;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.AdminContext.AdminDTOs;
public class AdminQuery : UserQuery
{
  public AdminQuery(int? skip, int? take, string name, string email, string phone, EGender? gender, DateTime? dateOfBirth, DateTime? createdAt, string? createdAtOperator,bool? isRootAdmin) : base(skip, take, name, email, phone, gender, dateOfBirth, createdAt, createdAtOperator)
  {
    IsRootAdmin = isRootAdmin;
  }

  public bool? IsRootAdmin { get; set; }
}

