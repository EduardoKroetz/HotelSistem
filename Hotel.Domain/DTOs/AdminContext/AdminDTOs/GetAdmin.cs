namespace Hotel.Domain.DTOs.AdminContext.AdminDTOs;

public class GetAdmin
{
  public GetAdmin(Guid id, string firstName, string lastName, string emailAddress, string phoneNumber, bool isRootAdmin, DateTime createdAt)
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
    Email = emailAddress;
    Phone = phoneNumber;
    IsRootAdmin = isRootAdmin;
    CreatedAt = createdAt;
  }
  public Guid Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
  public bool IsRootAdmin { get; set; }
  public DateTime CreatedAt { get; set; }


}
