using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

public class EditorCategory : IDataTransferObject
{
  public EditorCategory(string name, string description, decimal averagePrice)
  {
    Name = name;
    Description = description;
    AveragePrice = averagePrice;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public decimal AveragePrice { get; private set; }
}

