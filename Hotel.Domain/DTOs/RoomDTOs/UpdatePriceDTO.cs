namespace Hotel.Domain.DTOs.RoomDTOs;

public class UpdatePriceDTO : IDataTransferObject
{
    public UpdatePriceDTO(decimal price)
    {
        Price = price;
    }

    public decimal Price { get; private set; }
}
