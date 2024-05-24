using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.Base;

public abstract class User : Entity, IUser
{
  public User()
  {}
  
  public User(Name name, Email email, Phone phone, string password, EGender? gender = null, DateTime? dateOfBirth = null, Address? address = null)
  {
    Name = name;
    Email = email;
    Phone = phone;
    PasswordHash = PasswordHasher.HashPassword(password);
    Gender = gender;
    DateOfBirth = dateOfBirth;
    Address = address;
    IncompleteProfile = address == null || gender == null || dateOfBirth == null;
    
    Validate();
  }

  public Name Name { get; private set; } = null!;
  public Email Email { get; private set; } = null!;
  public Phone Phone { get; private set; } = null!;
  public string PasswordHash { get; private set; } = null!;
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

  public void ChangeGender(EGender? gender)
  {
    ValidateGender(gender);
    Gender = gender;
  }

  public void ChangeDateOfBirth(DateTime? birth)
  => DateOfBirth = birth; 
  public void CompleteProfile()
  => IncompleteProfile = Address == null || Gender == null || DateOfBirth == null;

  public override void Validate()
  {
    ValidateGender(Gender);

    base.Validate();
  }

  public void ValidateGender(EGender? gender)
  {
    //Verifica se é nulo e se é 1 ou 2 - Masculino/Feminino
    if (gender != null && ( (int)gender > 2 || (int)gender < 1) )
      throw new ValidationException("Erro de validação: Gênero inválido.");
  }

}