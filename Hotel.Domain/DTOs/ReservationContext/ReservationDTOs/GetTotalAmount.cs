using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class GetTotalAmount : IDataTransferObject
{
  public GetTotalAmount(decimal dailyRate, DateTime checkIn, DateTime checkOut, ICollection<Guid> services)
  {
    DailyRate = dailyRate;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Services = services ?? [];
  }

  public decimal DailyRate { get; private set; }
  public DateTime CheckIn { get; private set; }
  public DateTime CheckOut { get; private set; }
  public ICollection<Guid> Services { get; private set; }
}
