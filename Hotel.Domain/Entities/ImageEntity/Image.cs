using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Domain.Entities.ImageEntity;

public class Image : Entity
{
    internal Image() { }
    public Image(string url, Guid roomId)
    {
        Url = url;
        RoomId = roomId;

        Validate();
    }

    public string Url { get; private set; } = string.Empty;
    public Guid RoomId { get; private set; }
    public Room? Room { get; private set; }

}