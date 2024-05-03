using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public override void Validate()
  {
    ValidateCapacity(Capacity);
    ValidateCheckIn(CheckIn);
    ValidateCheckOut(CheckOut);

    if (CheckOut != null && CheckIn.Date > CheckOut.Value.Date)
      throw new ValidationException("Erro de validação: A data de check-in deve ser anterior à data de check-out.");

    base.Validate();
  }

  public void ValidateCapacity(int capacity)
  {
    if (capacity > Room.Capacity)
      throw new ValidationException("Erro de validação: Capacidade máxima de hospedades do quarto foi atingida.");

    if (capacity <= 0)
      throw new ValidationException("Erro de validação: Informe a quantidade de hóspedes de vão se hospedar.");
  }

  public void ValidateCheckIn(DateTime checkIn)
  {
    if (checkIn.Date < DateTime.Now.Date)
      throw new ValidationException("Erro de validação: A data de CheckIn não pode ser menor que a data atual.");
  }

  public void ValidateCheckOut(DateTime? checkOut)
  {
    if (checkOut != null)
      if (checkOut.Value.Date < DateTime.Now.Date)
        throw new ValidationException("Erro de validação: A data de CheckOut não pode ser menor que a data atual.");
  }
}