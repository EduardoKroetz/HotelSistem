using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public int CalculeHostedDays()
  {
    if (CheckOut != null)
      return (CheckOut.Value.Date - CheckIn.Date).Days;
    return 0;
  }

  public decimal TotalAmount()
  {
    if (CheckOut == null)
      throw new ValidationException("Erro de validação: É necessário a data de CheckOut para calcular o preço total.");
    var price = DailyRate * CalculeHostedDays();
    foreach (var service in Services)
      price += service.Price;
    return price;
  }


}