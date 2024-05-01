using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.CustomerContext;

public class Customer : User
{
  public Customer(Name name, Email email, Phone phone, string? passwordHash, EGender? gender, DateTime? dateOfBirth, Address? address) 
    : base(name,email,phone,passwordHash,gender,dateOfBirth,address)
  {
  }
}