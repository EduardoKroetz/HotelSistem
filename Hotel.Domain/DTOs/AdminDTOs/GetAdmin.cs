using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.DTOs.AdminDTOs;

public class GetAdmin : GetUser
{
    public GetAdmin(Guid id, string firstName, string lastName, string emailAddress, string phoneNumber, bool isRootAdmin, EGender? gender, DateTime? dateOfBirth, Address? address, DateTime createdAt) : base(id, firstName, lastName, emailAddress, phoneNumber, gender, dateOfBirth, address, createdAt)
    => IsRootAdmin = isRootAdmin;
    public bool IsRootAdmin { get; private set; }

}
