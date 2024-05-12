using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.Base.User;
public class GetUser : IDataTransferObject
{
    public GetUser(Guid id, string firstName, string lastName, string email, string phone, DateTime createdAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        CreatedAt = createdAt;
    }
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }

}