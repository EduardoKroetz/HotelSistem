using System.ComponentModel.DataAnnotations;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public class Employee : User, IResponsabilitiesMethods
{
  public Employee(Name name, Email email, Phone phone, string password, Responsability responsability, EGender? gender, DateTime? dateOfBirth, Address? address, decimal? salary) 
    : base( name, email, phone, password, gender, dateOfBirth, address)
  {
    Salary = salary;
    Responsabilities = [];
    AddResponsability(responsability);
  }
  
  public decimal? Salary { get; private set; }
  public List<Responsability> Responsabilities { get; private set; } 

  public void ChangeSalary(decimal salary)
  {
    if (salary <= 0)
      throw new  ValidationException("O sálario do funcionário não pode ser menor ou igual a zero.");
    Salary = salary;
  }

  public void AddResponsability(Responsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      Responsabilities.Add(responsability);
    else
      throw new ArgumentException("Esta responsabilidade já está atribuida à esse funcionário.");
  }

  public void RemoveResponsability(Responsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      Responsabilities.Remove(responsability);
    else
      throw new ArgumentException("Esta responsabilidade NÃO está atribuida à esse funcionário.");
  }

  
}