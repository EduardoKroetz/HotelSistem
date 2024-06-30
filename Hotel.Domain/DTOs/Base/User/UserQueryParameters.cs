using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.Base.User;
public class UserQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public EGender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? DateOfBirthOperator { get; set; }
}

