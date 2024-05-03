using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.Base;

public class User : Entity
{

  public User(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
  {
    Name = name;
    Email = email;
    Phone = phone;
    PasswordHash = GeneratePasswordHash(password);
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Address = address;
    IncompleteProfile = address == null || gender == null || dateOfBirth == null;
    
    Validate();
  }

  public Name Name { get; private set; }
  public Email Email { get; private set; }
  public Phone Phone { get; private set; }
  public string? PasswordHash { get; private set; }
  public EGender? Gender { get; private set; }
  public DateTime? DateOfBirth { get; private set; }
  public Address? Address { get; private set; }
  public bool IncompleteProfile { get; private set; }

  public void ChangeName(Name name)
  => Name = name; 

  public void ChangeEmail(Email email)
  => Email = email; 

  
  public void ChangePhone(Phone phone)
  => Phone = phone; 

  public void ChangeAddress(Address address)
  => Address = address; 

  public string GeneratePasswordHash(string Password)
  {
    //Implementar
    return "";
  }

}