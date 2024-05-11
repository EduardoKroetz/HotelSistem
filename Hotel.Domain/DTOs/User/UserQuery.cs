using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.User;
public class UserQuery
{
  public UserQuery(int? skip, int? take, string? name, string? email, string? phone, EGender? gender, DateTime? dateOfBirth, DateTime? createdAt, string? createdAtOperator)
  {
    Skip = skip;
    Take = take;
    Name = name;
    Email = email;
    Phone = phone;
    Gender = gender;
    DateOfBirth = dateOfBirth;
    CreatedAt = createdAt;
    CreatedAtOperator = createdAtOperator;
  }

  public int? Skip { get; set; }
  public int? Take { get; set; }
  public string? Name { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public EGender? Gender { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public DateTime? CreatedAt { get; set; }
  public string? CreatedAtOperator { get; set; }
}

