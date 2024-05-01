using Hotel.Domain.Entities.RoomContext.ImageEntity;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void AddImage(Image image)
  {
    image.Validate();
    if (Images.Contains(image))
      throw new ArgumentException("Essa imagem já está atribuida à este quarto.");
    Images.Add(image);
  }

  
  public void RemoveImage(Image image)
  {
    if (!Images.Contains(image))
      throw new ArgumentException("Essa imagem não está atribuida à este quarto.");
    Images.Remove(image);
  }
  
}