using Hotel.Domain.Entities.ImageEntity;

namespace Hotel.Domain.Entities.RoomEntity;

public partial class Room
{
    public Image CreateImage(string url)
    {
        var image = new Image(url, Id);
        Images.Add(image);
        return image;
    }

    public void DeleteImage(Image image)
    => Images.Remove(image);

}