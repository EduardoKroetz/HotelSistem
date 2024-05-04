using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities.RoomContext.CategoryEntity;

public partial class Category : Entity, ICategory
{
  public Category(string name, string description, decimal averagePrice)
  {
    Name = name;
    Description = description;
    AveragePrice = averagePrice;

    Validate();
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public decimal AveragePrice { get; private set; }
}