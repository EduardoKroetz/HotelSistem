using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.Base.User;
public class UserQueryParameters : QueryParameters
{
    public UserQueryParameters(int? skip, int? take, string? name, string? email, string? phone, EGender? gender, DateTime? dateOfBirth, DateTime? createdAt, string? createdAtOperator) : base(skip,take,createdAt,createdAtOperator)
    {
      Name = name;
      Email = email;
      Phone = phone;
      Gender = gender;
      DateOfBirth = dateOfBirth;
    }

    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public EGender? Gender { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
}

