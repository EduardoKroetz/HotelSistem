using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.AdminContext.AdminDTOs;

public class GetAdmin : IDataTransferObject
{
  public GetAdmin(Guid id ,string firstName, string lastName, string email, string phone)
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
    Email = email;
    Phone = phone;
  }
  public Guid Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }

}