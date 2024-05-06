using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.User;
public class UpdateUser : IDataTransferObject
{
  public UpdateUser(string firstName, string lastName, string email, string phone, EGender? gender, DateTime? dateOfBirth, string? country, string? city, string? street, int? number)
  {
    FirstName = firstName;
    LastName = lastName;
    Email = email;
    Phone = phone;
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Country = country;
    City = city;
    Street = street;
    Number = number;
  }

  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string Email { get; private set; }
  public string Phone { get; private set; }
  public EGender? Gender { get; private set; } 
  public DateTime? DateOfBirth { get; private set; }
  public string? Country { get; private set; }
  public string? City { get; private set; }
  public string? Street { get; private set; }
  public int? Number { get; private set; }
}