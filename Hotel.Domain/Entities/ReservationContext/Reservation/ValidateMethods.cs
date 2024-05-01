using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public override void Validate()
  {
    Room.Validate();

    if (Capacity > Room.Capacity)
      throw new ValidationException("Capacidade máxima de hospedades do quarto foi atingida.");

    if (Capacity <= 0)
      throw new ValidationException("Informe a quantidade de hóspedes.");

    if (CheckOut != null && CheckIn >= CheckOut)
      throw new ValidationException("A data de check-in deve ser anterior à data de check-out.");

    ValidateCheckIn(CheckIn);
    ValidateCheckOut(CheckOut);

    base.Validate();
  }


  public void ValidateCheckIn(DateTime checkIn)
  {
    if (checkIn < DateTime.Now)
      throw new ValidationException("A data de CheckIn não pode ser menor que a data atual.");
  }

  public void ValidateCheckOut(DateTime? checkOut)
  {
    if (checkOut != null)
      if (checkOut < DateTime.Now)
        throw new ValidationException("A data de CheckOut não pode ser menor que a data atual.");
  }
}