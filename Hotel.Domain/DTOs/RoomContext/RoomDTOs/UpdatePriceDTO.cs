using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class UpdatePriceDTO : IDataTransferObject
{
  public UpdatePriceDTO(decimal price)
  {
    Price = price;
  }

  public decimal Price { get; private set; }
}
