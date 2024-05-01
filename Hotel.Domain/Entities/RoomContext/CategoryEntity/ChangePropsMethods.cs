namespace Hotel.Domain.Entities.RoomContext.CategoryEntity;

public partial class Category
{
  public void ChangeName(string name)
  => Name = name;

  public void ChangeDescription(string description)
  => Description = description;

  public void ChangeAveragePrice(decimal averagePrice)
  => AveragePrice = averagePrice;
}