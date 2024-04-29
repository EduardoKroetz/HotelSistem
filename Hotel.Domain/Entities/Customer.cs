using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities;

public class Customer : Entity
{
  public Customer(Name name, string email, string phone, string passwordHash, EGender gender, DateTime dateOfBirth, Address address)
  {
    Name = name;
    Email = email;
    Phone = phone;
    PasswordHash = passwordHash;
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Address = address;
  }

  public Name Name { get; private set; }
  public string Email { get; private set; }
  public string Phone { get; private set; }
  public string PasswordHash { get; private set; }
  public EGender Gender { get; private set; }
  public DateTime DateOfBirth { get; private set; }
  public Address Address { get; private set; }
}