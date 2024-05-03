using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void ChangeStatus(ERoomStatus status)
  => Status = status;

  public void ChangeNumber(int number)
  {
    ValidateNumber(number);
    Number = number;
  }


  public void ChangePrice(decimal price)
  {
    ValidatePrice(price);
    Price = price;
  }

  public void ChangeCapacity(int capacity)
  {
    ValidateCapacity(capacity);
    Capacity = capacity;
  }

}