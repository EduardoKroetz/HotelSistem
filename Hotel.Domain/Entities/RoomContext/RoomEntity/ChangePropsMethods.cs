using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void ChangeStatus(ERoomStatus status)
  => Status = status;

  public void ChangeNumber(int number)
  => Number = number;

  public void ChangePrice(decimal price)
  => Price = price;
}