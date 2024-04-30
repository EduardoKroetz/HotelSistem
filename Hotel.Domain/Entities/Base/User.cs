using Hotel.Domain.Entities.Validation;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.Base;

public class User : Entity ,IValidation
{

  public User(Name name, Email email, Phone phone, string passwordHash, EGender gender, DateTime dateOfBirth, Address address)
  {
    Name = name;
    Email = email;
    Phone = phone;
    PasswordHash = passwordHash;
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Address = address;
    Validate();
  }

  public Name Name { get; private set; }
  public Email Email { get; private set; }
  public Phone Phone { get; private set; }
  public string PasswordHash { get; private set; }
  public EGender Gender { get; private set; }
  public DateTime DateOfBirth { get; private set; }
  public Address Address { get; private set; }

  public bool IsValid { get; private set; } = false;

  public virtual void Validate()
  {
    Name.Validate();
    Email.Validate();
    Phone.Validate();
    Address.Validate();

    IsValid = true;
  }

  public void ChangeName(Name name)
  => Name = name; 

  public void ChangeEmail(Email email)
  => Email = email; 

  
  public void ChangePhone(Phone phone)
  => Phone = phone; 

  public void ChangeAddress(Address address)
  => Address = address; 

}