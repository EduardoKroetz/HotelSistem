using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;

public partial class InvoiceRoom
{
  public override void Validate()
  {
    if (Reservation?.Status != EReservationStatus.CheckedOut)
      throw new ValidationException("Erro de validação: A Reserva não finalizada.");

    ValidateTaxInformation(TaxInformation);
    TotalAmount += TaxInformation;

    base.Validate();
  }

  public void ValidateTaxInformation(decimal taxInformation)
  {
    if (taxInformation < 0)
      throw new ValidationException("Erro de validação: O imposto da fatura não pode ser menor que 0.");
  }
  
}