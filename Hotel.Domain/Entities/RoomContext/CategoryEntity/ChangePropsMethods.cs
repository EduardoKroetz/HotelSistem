namespace Hotel.Domain.Entities.RoomContext.CategoryEntity;

public partial class Category
{
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }

  public void ChangeDescription(string description)
  {
    ValidateDescription(description);
    Description = description;
  }

  public void ChangeAveragePrice(decimal averagePrice)
  {
    ValidateAveragePrice(averagePrice);
    AveragePrice = averagePrice;
  }

}