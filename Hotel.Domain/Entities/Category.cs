using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities;

public class Category : Entity
{
  public Category(string name, string description, decimal averagePrice)
  {
    Name = name;
    Description = description;
    AveragePrice = averagePrice;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public decimal AveragePrice { get; private set; }
}