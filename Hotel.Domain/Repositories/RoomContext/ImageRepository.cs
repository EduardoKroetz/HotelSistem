using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Repositories.Interfaces.RoomContext;

namespace Hotel.Domain.Repositories.RoomContext;

public class ImageRepository : GenericRepository<Image>, IImageRepository
{
  public ImageRepository(HotelDbContext context) : base(context) { }

}