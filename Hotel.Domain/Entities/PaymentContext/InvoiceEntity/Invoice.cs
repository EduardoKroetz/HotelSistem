using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.PaymentContext.InvoiceEntity;

public class Invoice : Entity
{
  public Invoice(int number, decimal totalAmount, EPaymentMethod paymentMethod, decimal? taxInformation)
  {
    Number = number;
    IssueDate = DateTime.Now;
    TotalAmount = totalAmount;
    Status = EStatus.Pending;
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    Customers = [];
  }

  public int Number { get; private set; }
  public DateTime IssueDate { get; private set; }
  public decimal TotalAmount { get; private set; }
  public EStatus Status { get; private set; }
  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal? TaxInformation { get; private set; }
  public List<Customer> Customers { get; private set; }
}