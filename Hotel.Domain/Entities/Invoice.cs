using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class Invoice : Entity
{
  public Invoice(int number, DateTime issueDate, decimal totalAmount, EStatus status, EPaymentMethod paymentMethod, decimal taxInformation)
  {
    Number = number;
    IssueDate = issueDate;
    TotalAmount = totalAmount;
    Status = status;
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    Customers = [];
  }

  public int Number { get; private set; }
  public DateTime IssueDate { get; private set; }
  public decimal TotalAmount { get; private set; }
  public EStatus Status { get; private set; }
  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal TaxInformation { get; private set; }
  public List<Customer> Customers { get; private set; }
}