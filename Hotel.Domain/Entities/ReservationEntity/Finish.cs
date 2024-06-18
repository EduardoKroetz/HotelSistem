using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

public partial class Reservation
{
    public Invoice Finish(EPaymentMethod paymentMethod, decimal taxInformation = 0)
    {
        if (CheckIn == null)
            throw new ArgumentNullException("Não foi possível finalizar a reserva pois o CheckIn ainda não foi realizado.");

        if (DateTime.Now.Date < CheckIn.Value.Date)
            throw new ValidationException("Não é possível gerar a fatura pois a data de CheckIn é maior que a data atual.");

        //Troca o CheckOut para a data atual, já que está finalizando
        UpdateCheckOut(DateTime.Now)
          .ToCheckOut(); // Muda o status para CheckedOut

        Invoice = new Invoice(paymentMethod, this, taxInformation); //Gera uma instância de uma fatura

        Room?.ChangeStatus(ERoomStatus.OutOfService); //troca o status do quarto para 'OutOfService'(Fora de serviço)

        return Invoice;
    }

}