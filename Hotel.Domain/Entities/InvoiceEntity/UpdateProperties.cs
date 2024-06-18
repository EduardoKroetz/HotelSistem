
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.InvoiceEntity;

public partial class Invoice
{
    public void FinishInvoice()
    {
        Status = EStatus.Finish;
    }

}