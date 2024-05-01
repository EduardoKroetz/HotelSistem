using Hotel.Domain.Entities.RoomContext.ImageEntity;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void AddImage(Image image)
  {
    image.Validate();
    Images.Add(image);
  }

  
  public void RemoveImage(Image image)
  => Images.Remove(image);
}