using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{
    public static decimal TotalAmount(decimal? dailyRate, DateTime? checkIn, DateTime? checkOut, ICollection<Service> services)
    {
        _ =
        dailyRate is null ?
          throw new ValidationException("Valor da diária inválido.") :
        checkIn is null ?
            throw new ValidationException("CheckIn inválido.") :
        checkOut is null ?
            throw new ValidationException("CheckOut inválido.") : 0;

        ValidateCheckInAndCheckOut(checkIn, checkOut);

        //Calcula o tempo que ficou hospedado
        var timeHosted = checkOut - checkIn;

        //Taxa por minuto
        var rateMinute = ( dailyRate / 24 ) / 60;

        //Calcula o preço total da estadia
        var price = rateMinute * (decimal)timeHosted?.TotalMinutes!;

        //Adiciona os preços dos serviços adicionais
        foreach (var service in services)
            price += service.Price;

        return (decimal)price;
    }

    public decimal TotalAmount()
    {
        return TotalAmount(DailyRate, CheckIn, CheckOut, Services);
    }

    public decimal ExpectedTotalAmount()
    {
        return TotalAmount(DailyRate, ExpectedCheckIn, ExpectedCheckOut, Services);
    }

}