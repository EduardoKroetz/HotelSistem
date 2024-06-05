using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public static TimeSpan? GetTimeHosted(DateTime checkIn, DateTime? checkOut)
  {
    var duration = checkOut - checkIn;
    return duration;
  }

  public  TimeSpan? GetTimeHosted()
  {
    var duration = CheckOut - CheckIn;
    return duration;
  }

  public static decimal TotalAmount(decimal? dailyRate, DateTime? checkIn, DateTime? checkOut, ICollection<Service> services)
  {
    _ = 
    dailyRate is null ?
      throw new ValidationException("Erro de validação: Valor da diária inválido.") :
    checkIn is null ?
        throw new ValidationException("Erro de validação: CheckIn inválido.") :
    checkOut is null ?
        throw new ValidationException("Erro de validação: CheckOut inválido.") : 0;

    //Calcula o tempo que ficou hospedado
    var timeHosted = checkOut - checkIn;

    //Taxa por minuto
    var rateMinute = (dailyRate / 24) / 60;

    //Calcula o preço total da estadia
    var price = rateMinute *  (decimal)timeHosted?.TotalMinutes!;

    //Adiciona os preços dos serviços adicionais
    foreach (var service in services)
      price += service.Price;

    return (decimal)price;
  }

  public decimal TotalAmount()
  {
    return TotalAmount(DailyRate, CheckIn, CheckOut, Services);
  }


}