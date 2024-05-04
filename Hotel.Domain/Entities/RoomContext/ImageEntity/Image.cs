using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Entities.RoomContext.ImageEntity;

public class Image : Entity
{
  private Image(){}
  public Image(string url, Guid roomId)
  {
    Url = url;
    RoomId = roomId;

    Validate();
  }

  public string Url { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; } 

}