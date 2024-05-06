using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.User;
public class GetUser : IDataTransferObject
{
  public GetUser(Guid id ,string firstName, string lastName, string email, string phone)
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