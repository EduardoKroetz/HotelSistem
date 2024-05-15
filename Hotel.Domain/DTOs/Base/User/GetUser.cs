using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.DTOs.Base.User;
public class GetUser : IDataTransferObject
{
  public GetUser(Guid id, string firstName, string lastName, string email, string phone, EGender? gender, DateTime? dateOfBirth, Address? address, DateTime createdAt)
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
    Email = email;
    Phone = phone;
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Address = address;
    CreatedAt = createdAt;
  }

  public Guid Id { get; private set; }
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string Email { get; private set; }
  public string Phone { get; private set; }
  public EGender? Gender { get; private set; }
  public DateTime? DateOfBirth { get; private set; }
  public Address? Address { get; private set; }
  public DateTime CreatedAt { get; private set; }

}