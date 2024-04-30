using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities;

public class Image : Entity
{
  public Image(string url, Guid roomId)
  {
    Url = url;
    RoomId = roomId;
  }

  public string Url { get; private set; }
  public Guid RoomId { get; private set; }
}