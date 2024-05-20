namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class UpdateRate
{
  public UpdateRate(int rate)
  => Rate = rate;
       
  public int Rate { get; private set; }
}
