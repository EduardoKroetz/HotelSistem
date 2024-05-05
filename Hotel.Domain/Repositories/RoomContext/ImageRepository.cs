using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class ImageRepository : GenericRepository<Image> ,IImageRepository
{
  public ImageRepository(HotelDbContext context) : base(context) {}

}