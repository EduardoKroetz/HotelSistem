using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public override void Validate()
  {
    ValidateCapacity(Capacity);
    ValidateCheckIn(CheckIn);
    ValidateRoom(Room);
    ValidateCheckIn(ExpectedCheckIn);
    ValidateCheckInAndCheckOut(ExpectedCheckIn, ExpectedCheckOut);

    base.Validate();
  }

  public void ValidateCapacity(int capacity)
  {
    if (capacity > Room?.Capacity)
      throw new ValidationException("Capacidade máxima de hospedades do cômodo excedida.");

    if (capacity <= 0)
      throw new ValidationException("Informe a quantidade de hóspedes de vão se hospedar.");
  }

  public void ValidateCheckIn(DateTime? checkIn)
  {
    if (checkIn is not null)
    {
      if (checkIn is not null && checkIn?.Date < DateTime.Now.Date)
        throw new ValidationException("A data de CheckIn não pode ser menor que a data atual.");
    }
  }

  public static void ValidateCheckInAndCheckOut(DateTime? checkIn, DateTime? checkOut)
  {
    if (checkOut is not null && checkIn is not null)
    {
      if (checkIn > checkOut.Value)
        throw new ValidationException("A data de check-out deve ser maior que a data de check-in.");
    }
  }

  public void ValidateRoom(Room? room)
  {
    if (room == null)
      throw new ArgumentException("Cômodo inválido.");
    else if (room.IsActive is false)
      throw new InvalidOperationException("Não é possível realizar a reserva pois o cômodo está inativo.");
    else if (room?.Status != Enums.ERoomStatus.Available)
      throw new InvalidOperationException("Não é possível realizar a reserva pois o cômodo está indisponível.");

  }
}