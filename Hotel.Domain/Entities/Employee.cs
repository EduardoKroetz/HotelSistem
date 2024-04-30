using System.ComponentModel.DataAnnotations;
using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities;

public class Employee : User, IResponsabilities
{
  public Employee(Name name, Email email, Phone phone, string passwordHash, EGender gender, DateTime dateOfBirth, Address address,decimal salary,
    EmployeeResponsability responsability) 
    : base( name, email, phone, passwordHash, gender, dateOfBirth, address)
  {
    Salary = salary;
    Responsabilities = [];
    AddResponsability(responsability);
  }
  
  public decimal Salary { get; private set; }
  public List<EmployeeResponsability> Responsabilities { get; private set; } 

  public void ChangeSalary(decimal salary)
  {
    if (salary <= 0)
      throw new  ValidationException("O sálario do funcionário não pode ser menor ou igual a zero.");
    Salary = salary;
  }

  public void AddResponsability(EmployeeResponsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      Responsabilities.Add(responsability);
    else
      throw new ArgumentException("Esta responsabilidade já está atribuida à esse funcionário.");
  }

  public void RemoveResponsability(EmployeeResponsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      Responsabilities.Remove(responsability);
    else
      throw new ArgumentException("Esta responsabilidade NÃO está atribuida à esse funcionário.");
  }

  
}