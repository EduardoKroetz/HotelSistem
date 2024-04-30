using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class Service : Entity
{
  public Service(string name, decimal price, bool isActive, EPriority priority, DateTime time, Guid empRespId)
  {
    Name = name;
    Price = price;
    IsActive = isActive;
    Priority = priority;
    Time = time;
    EmpRespId = empRespId;
  }

  public string Name { get; private set; }
  public decimal Price { get; private set; }
  public bool IsActive { get; private set; }
  public EPriority Priority { get; private set; }
  public DateTime Time { get; private set; }
  public Guid EmpRespId { get; private set; }
  public EmployeeResponsability? Responsability { get; private set; } 
}