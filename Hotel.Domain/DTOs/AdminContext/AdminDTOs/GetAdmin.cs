using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.DTOs.AdminContext.AdminDTOs;

public class GetAdmin : GetUser
{
  public GetAdmin(Guid id, string firstName, string lastName, string emailAddress, string phoneNumber, bool isRootAdmin, DateTime createdAt) : base(id,firstName,lastName,emailAddress,phoneNumber,createdAt)
  => IsRootAdmin = isRootAdmin;
  public bool IsRootAdmin { get; private set; }

}
