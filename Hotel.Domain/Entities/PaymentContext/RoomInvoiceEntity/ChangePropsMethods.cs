
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;

public partial class InvoiceRoom
{
  public void FinishInvoice()
  {
    Status = EStatus.Finish;
    //Envio de email
  }
  
}