namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class UpdateFeedback
{
  public UpdateFeedback(string comment, int rate)
  {
    Comment = comment;
    Rate = rate;
  }

  public string Comment { get; set; }
  public int Rate { get; set; }
}

