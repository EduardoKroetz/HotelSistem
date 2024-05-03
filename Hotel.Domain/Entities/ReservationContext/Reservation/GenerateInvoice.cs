using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

public partial class Reservation 
{
  public InvoiceRoom GenerateInvoice(EPaymentMethod paymentMethod,decimal taxInformation = 0)
  {
    if (DateTime.Now.Date < CheckIn)
      throw new ValidationException("Erro de validação: Não é possível gerar a fatura pois a data de CheckIn é maior que a data atual.");
    ChangeCheckOut(DateTime.Now).StatusToCheckedOut();
    Invoice = new InvoiceRoom(paymentMethod,this,taxInformation);
    Room.ChangeStatus(ERoomStatus.OutOfService);
    return Invoice;
  }

}