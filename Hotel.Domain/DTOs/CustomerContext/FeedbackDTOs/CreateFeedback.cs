using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class CreateFeedback : IDataTransferObject
{
  public CreateFeedback(string comment, int rate, Guid reservationId)
  {
    Comment = comment;
    Rate = rate;
    ReservationId = reservationId;
  }

  public string Comment { get; private set; }
  public int Rate { get; private set; }
  public Guid ReservationId { get; private set; }
}

