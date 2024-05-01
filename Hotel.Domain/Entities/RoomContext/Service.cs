using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext;
using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext;

public class Service : Entity, IResponsabilities
{
  public Service(string name, decimal price, bool isActive, EPriority priority, DateTime time, EmployeeResponsability responsability)
  {
    Name = name;
    Price = price;
    IsActive = isActive;
    Priority = priority;
    Time = time;
    Responsabilities = [];

    AddResponsability(responsability);
  }

  public string Name { get; private set; }
  public decimal Price { get; private set; }
  public bool IsActive { get; private set; }
  public EPriority Priority { get; private set; }
  public DateTime Time { get; private set; }
  public List<EmployeeResponsability> Responsabilities { get; private set; } 

  
  public void AddResponsability(EmployeeResponsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      Responsabilities.Add(responsability);
    else
      throw new ArgumentException("Esta responsabilidade já está atribuida à esse serviço.");
  }

  public void RemoveResponsability(EmployeeResponsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      Responsabilities.Remove(responsability);
    else
      throw new ArgumentException("Esta responsabilidade NÃO está atribuida à esse serviço.");
  }

}