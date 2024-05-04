using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities.RoomContext.ImageEntity;

public class Image : Entity
{
  public Image(string url, Guid roomId)
  {
    Url = url;
    RoomId = roomId;

    Validate();
  }

  public string Url { get; private set; }
  public Guid RoomId { get; private set; }
}