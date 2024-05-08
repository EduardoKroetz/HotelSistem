using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class GetCategory : IDataTransferObject
{
  public GetCategory(Guid id ,string name, string description, decimal averagePrice)
  {
    Id = id;
    Name = name;
    Description = description;
    AveragePrice = averagePrice;
  }

  public Guid Id { get; private set; } 
  public string Name { get; private set; } 
  public string Description { get; private set; } 
  public decimal AveragePrice { get; private set; }
} 
  
